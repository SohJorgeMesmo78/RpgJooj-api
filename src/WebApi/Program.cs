using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext using PostgreSQL connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Infrastructure")));

// Enable CORS for Angular frontend local server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");

// Execute migrations and seed database automatically at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao executar a migration do banco de dados.");
    }
}

// REST Endpoints
app.MapGet("/api/personagens", async (AppDbContext db) =>
{
    var personagens = await db.Personagens
        .Include(p => p.ClassesPersonagens)
            .ThenInclude(cp => cp.Classe)
        .Include(p => p.ClassesPersonagens)
            .ThenInclude(cp => cp.Subclasse)
        .Select(p => new
        {
            p.Id,
            p.Nome,
            p.Codigo,
            Raca = p.Raca.Nome,
            Classe = p.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Classe.Nome).FirstOrDefault() ?? "Sem Classe",
            Subclasse = p.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Subclasse != null ? cp.Subclasse.Nome : null).FirstOrDefault(),
            Nivel = p.ClassesPersonagens.Sum(cp => cp.Nivel),
            p.Base64Imagem
        })
        .ToListAsync();
    return Results.Ok(personagens);
})
.WithName("GetPersonagens");

app.MapGet("/api/personagens/{codigo}", async (string codigo, AppDbContext db) =>
{
    var personagem = await db.Personagens
        .Include(p => p.Historias.OrderBy(h => h.Capitulo))
        .Include(p => p.ClassesPersonagens)
            .ThenInclude(cp => cp.Classe)
                .ThenInclude(c => c.Progressoes)
        .Include(p => p.ClassesPersonagens)
            .ThenInclude(cp => cp.Classe)
                .ThenInclude(c => c.Caracteristicas)
        .Include(p => p.ClassesPersonagens)
            .ThenInclude(cp => cp.Subclasse)
        .Include(p => p.PersonagensPericias)
        .Include(p => p.Raca)
            .ThenInclude(r => r.TracosRaciais)
                .ThenInclude(t => t.TracosRaciaisPericias)
        .Include(p => p.Acoes)
        .Include(p => p.Proficiencias)
        .Include(p => p.Salvaguardas)
        .Include(p => p.PersonagensIdiomas)
            .ThenInclude(pi => pi.Idioma)
        .Include(p => p.PersonagensMagias)
            .ThenInclude(pm => pm.Magia)
        .FirstOrDefaultAsync(p => p.Codigo.ToLower() == codigo.ToLower());

    if (personagem == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    var todasPericias = await db.Pericias.OrderBy(p => p.Nome).ToListAsync();

    var nivelTotal = personagem.ClassesPersonagens.Sum(cp => cp.Nivel);
    var bonusProficiencia = 2 + (nivelTotal - 1) / 4;

    int ObterModificador(int valor) => (int)Math.Floor((valor - 10) / 2.0);

    var mods = new Dictionary<string, int>
    {
        { "Força", ObterModificador(personagem.Forca) },
        { "Destreza", ObterModificador(personagem.Destreza) },
        { "Constituição", ObterModificador(personagem.Constituicao) },
        { "Inteligência", ObterModificador(personagem.Inteligencia) },
        { "Sabedoria", ObterModificador(personagem.Sabedoria) },
        { "Carisma", ObterModificador(personagem.Carisma) }
    };

    var salvaguardasObj = new[] { "Força", "Destreza", "Constituição", "Inteligência", "Sabedoria", "Carisma" }.Select(attr =>
    {
        var dbKey = attr switch
        {
            "Força" => "Forca",
            "Constituição" => "Constituicao",
            "Inteligência" => "Inteligencia",
            _ => attr
        };
        var isProf = personagem.Salvaguardas.Any(s => s.Atributo.Equals(dbKey, StringComparison.OrdinalIgnoreCase) && s.IsProficiente);
        var mod = mods[attr];
        var valor = mod + (isProf ? bonusProficiencia : 0);
        return new
        {
            Atributo = attr,
            Modificador = mod,
            IsProficiente = isProf,
            Valor = valor
        };
    }).ToList();

    return Results.Ok(new
    {
        personagem.Id,
        personagem.Nome,
        personagem.Codigo,
        personagem.Base64Imagem,
        Raca = personagem.Raca.Nome,
        RacaInfo = new
        {
            personagem.Raca.Nome,
            personagem.Raca.Descricao,
            personagem.Raca.TipoCriatura,
            personagem.Raca.Tamanho,
            personagem.Raca.Deslocamento
        },
        TracosRaciais = personagem.Raca.TracosRaciais.Select(t => new
        {
            t.Id,
            t.Nome,
            t.Descricao
        }).ToList(),
        Classe = personagem.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Classe.Nome).FirstOrDefault() ?? "Sem Classe",
        Subclasse = personagem.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Subclasse != null ? cp.Subclasse.Nome : null).FirstOrDefault(),
        Nivel = personagem.ClassesPersonagens.Sum(cp => cp.Nivel),
        personagem.Alinhamento,
        personagem.Forca,
        personagem.Destreza,
        personagem.Constituicao,
        personagem.Inteligencia,
        personagem.Sabedoria,
        personagem.Carisma,
        personagem.VidaMaxima,
        personagem.VidaAtual,
        Classes = personagem.ClassesPersonagens.Select(cp => new
        {
            cp.Classe.Nome,
            Subclasse = cp.Subclasse != null ? cp.Subclasse.Nome : null,
            cp.Classe.DadoVida,
            cp.Classe.Deslocamento,
            cp.Nivel,
            Progresso = cp.Classe.Progressoes
                .Where(prog => prog.Nivel == cp.Nivel)
                .Select(prog => new
                {
                    prog.Nivel,
                    prog.BonusProficiencia,
                    prog.TruquesConhecidos,
                    prog.MagiasConhecidas,
                    prog.EspacosMagia,
                    prog.NivelMagia,
                    prog.InvocacoesConhecidas
                })
                .FirstOrDefault(),
            Caracteristicas = cp.Classe.Caracteristicas
                .Where(car => car.Nivel <= cp.Nivel)
                .Select(car => new
                {
                    car.Id,
                    car.Nivel,
                    car.Nome,
                    car.Descricao
                }).ToList()
        }),
        Historias = personagem.Historias.Select(h => new
        {
            h.Id,
            h.Capitulo,
            h.TituloCapitulo,
            h.Texto
        }),
        Pericias = todasPericias.Select(per =>
        {
            var pp = personagem.PersonagensPericias.FirstOrDefault(x => x.IdPericia == per.Id);
            var isProficienteManual = pp?.IsProficiente ?? false;
            var isMaestriaManual = pp?.IsMaestria ?? false;

            var tracoRacialQueDaPericia = personagem.Raca.TracosRaciais
                .FirstOrDefault(t => t.TracosRaciaisPericias.Any(trp => trp.IdPericia == per.Id));
            var isProficienteRacial = tracoRacialQueDaPericia != null;
            
            var isMaestriaRacial = tracoRacialQueDaPericia != null && tracoRacialQueDaPericia.TracosRaciaisPericias
                .Any(trp => trp.IdPericia == per.Id && trp.IsMaestria);

            var isProf = isProficienteManual || isProficienteRacial;
            var isMaestria = isMaestriaManual || isMaestriaRacial;

            string? origem = null;
            if (isProficienteRacial)
            {
                origem = $"Raça ({personagem.Raca.Nome}) - Traço ({tracoRacialQueDaPericia!.Nome})";
            }
            else if (isProficienteManual)
            {
                origem = pp?.Origem;
            }

            return new
            {
                Id = per.Id,
                Nome = per.Nome,
                ModificadorAtributo = per.ModificadorAtributo,
                Proficiente = isProf,
                Maestria = isMaestria,
                Origem = origem
            };
        }).ToList(),
        Salvaguardas = salvaguardasObj,
        Acoes = personagem.Acoes.Select(a => new
        {
            a.Id,
            a.Nome,
            a.TipoAcao,
            a.Alcance,
            a.BonusAcerto,
            a.Dano,
            a.TipoDano,
            a.Descricao,
            a.AcertoTooltip,
            a.DanoTooltip
        }).ToList(),
        Proficiencias = personagem.Proficiencias.Select(p => new
        {
            p.Id,
            p.Tipo,
            p.Nome,
            p.Origem
        }).ToList(),
        Idiomas = personagem.PersonagensIdiomas.Select(pi => pi.Idioma.Nome).ToList(),
        Magias = personagem.PersonagensMagias.Select(pm => new
        {
            pm.Magia.Id,
            pm.Magia.Nome,
            pm.Magia.Nivel,
            pm.Magia.Descricao
        }).ToList()
    });
})
.WithName("GetPersonagemByCodigo");

app.MapGet("/api/pericias", async (AppDbContext db) =>
{
    var pericias = await db.Pericias.OrderBy(p => p.Nome).ToListAsync();
    return Results.Ok(pericias);
})
.WithName("GetPericias");

app.MapPost("/api/personagens", async (PersonagemDto dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Codigo) ||
        string.IsNullOrWhiteSpace(dto.Raca) || string.IsNullOrWhiteSpace(dto.Classe) ||
        string.IsNullOrWhiteSpace(dto.Subclasse))
    {
        return Results.BadRequest(new { Message = "Nome, Código, Raça, Classe e Subclasse são obrigatórios." });
    }

    var exists = await db.Personagens.AnyAsync(p => p.Codigo.ToLower() == dto.Codigo.ToLower());
    if (exists)
    {
        return Results.Conflict(new { Message = $"Já existe um personagem com o código '{dto.Codigo}'." });
    }

    var racaDb = await db.Racas.FirstOrDefaultAsync(r => r.Nome.ToLower() == dto.Raca.ToLower());
    if (racaDb == null)
    {
        racaDb = new Raca
        {
            Nome = dto.Raca,
            Descricao = $"Raça {dto.Raca} criada automaticamente.",
            TipoCriatura = "Humanóide",
            Tamanho = "Médio",
            Deslocamento = 9
        };
        db.Racas.Add(racaDb);
        await db.SaveChangesAsync();
    }

    var personagem = new Personagem
    {
        Nome = dto.Nome,
        Codigo = dto.Codigo,
        Base64Imagem = dto.Base64Imagem,
        IdRaca = racaDb.Id,
        Alinhamento = dto.Alinhamento,
        Forca = dto.Forca,
        Destreza = dto.Destreza,
        Constituicao = dto.Constituicao,
        Inteligencia = dto.Inteligencia,
        Sabedoria = dto.Sabedoria,
        Carisma = dto.Carisma,
        VidaMaxima = dto.VidaMaxima > 0 ? dto.VidaMaxima : 10,
        VidaAtual = dto.VidaAtual > 0 ? dto.VidaAtual : (dto.VidaMaxima > 0 ? dto.VidaMaxima : 10)
    };

    db.Personagens.Add(personagem);
    await db.SaveChangesAsync();

    // Mapeamento relacional da Classe fornecida no DTO
    var classeDb = await db.Classes.FirstOrDefaultAsync(c => c.Nome.ToLower() == dto.Classe.ToLower() && c.IdClassePai == null);
    if (classeDb == null)
    {
        classeDb = new Classe
        {
            Nome = dto.Classe,
            IdClassePai = null,
            DadoVida = "d8",
            Deslocamento = null
        };
        db.Classes.Add(classeDb);
        await db.SaveChangesAsync();
    }

    var subclasseDb = await db.Classes.FirstOrDefaultAsync(c => c.Nome.ToLower() == dto.Subclasse.ToLower() && c.IdClassePai == classeDb.Id);
    if (subclasseDb == null)
    {
        subclasseDb = new Classe
        {
            Nome = dto.Subclasse,
            IdClassePai = classeDb.Id,
            DadoVida = "",
            Deslocamento = null
        };
        db.Classes.Add(subclasseDb);
        await db.SaveChangesAsync();
    }

    var cp = new ClassePersonagem
    {
        IdPersonagem = personagem.Id,
        IdClasse = classeDb.Id,
        IdSubclasse = subclasseDb.Id,
        Nivel = dto.Nivel > 0 ? dto.Nivel : 1
    };
    db.ClassesPersonagens.Add(cp);
    await db.SaveChangesAsync();

    return Results.Created($"/api/personagens/{personagem.Codigo}", new
    {
        personagem.Id,
        personagem.Nome,
        personagem.Codigo,
        personagem.Base64Imagem,
        Raca = racaDb.Nome,
        Classe = classeDb.Nome,
        Subclasse = subclasseDb.Nome,
        Nivel = cp.Nivel,
        personagem.Alinhamento,
        personagem.VidaMaxima,
        personagem.VidaAtual
    });
})
.WithName("CreatePersonagem");

app.Run();

public record PersonagemDto(
    string Nome, 
    string Codigo, 
    string? Base64Imagem, 
    string Raca, 
    string Classe, 
    string Subclasse, 
    int Nivel, 
    string? Alinhamento,
    int Forca = 10,
    int Destreza = 10,
    int Constituicao = 10,
    int Inteligencia = 10,
    int Sabedoria = 10,
    int Carisma = 10,
    int VidaMaxima = 10,
    int VidaAtual = 10
);
