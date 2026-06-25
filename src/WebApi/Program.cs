using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext using PostgreSQL connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Infrastructure"))
           .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

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
        .AsNoTracking()
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
    var codeLower = codigo.ToLower();
    var personagem = await db.Personagens
        .AsNoTracking()
        .AsSplitQuery()
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
        .FirstOrDefaultAsync(p => p.Codigo == codeLower);

    if (personagem == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    var todasPericias = await db.Pericias.AsNoTracking().OrderBy(p => p.Nome).ToListAsync();

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

app.MapGet("/api/personagens/detalhes", async (string codigo, AppDbContext db) =>
{
    var codeLower = codigo.ToLower();
    var p = await db.Personagens
        .AsNoTracking()
        .Select(x => new
        {
            x.Id,
            x.Nome,
            x.Codigo,
            x.Base64Imagem,
            Raca = x.Raca.Nome,
            Classes = x.ClassesPersonagens.Select(cp => new { cp.Classe.Nome, Subclasse = cp.Subclasse != null ? cp.Subclasse.Nome : null, cp.Nivel }),
            x.Alinhamento,
            x.Forca,
            x.Destreza,
            x.Constituicao,
            x.Inteligencia,
            x.Sabedoria,
            x.Carisma,
            x.VidaMaxima,
            x.VidaAtual,
            PrimeiroCapitulo = x.Historias.OrderBy(h => h.Capitulo).Select(h => h.Texto).FirstOrDefault()
        })
        .FirstOrDefaultAsync(x => x.Codigo == codeLower);

    if (p == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    var classStr = p.Classes.OrderBy(cp => cp.Nome).Select(cp => cp.Subclasse != null ? $"{cp.Nome} / {cp.Subclasse}" : cp.Nome).FirstOrDefault() ?? "Sem Classe";

    return Results.Ok(new
    {
        p.Id,
        p.Nome,
        p.Codigo,
        p.Base64Imagem,
        Raca = p.Raca,
        ClassAndSubclass = classStr,
        Nivel = p.Classes.Sum(cp => cp.Nivel),
        p.Alinhamento,
        p.Forca,
        p.Destreza,
        p.Constituicao,
        p.Inteligencia,
        p.Sabedoria,
        p.Carisma,
        p.VidaMaxima,
        p.VidaAtual,
        History = p.PrimeiroCapitulo ?? "História em desenvolvimento. Em breve novos registros."
    });
})
.WithName("GetPersonagemDetalhes");

app.MapGet("/api/personagens/historia", async (string codigo, AppDbContext db) =>
{
    var codeLower = codigo.ToLower();
    var p = await db.Personagens
        .AsNoTracking()
        .Include(x => x.Historias.OrderBy(h => h.Capitulo))
        .FirstOrDefaultAsync(x => x.Codigo == codeLower);

    if (p == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    return Results.Ok(new
    {
        p.Id,
        p.Nome,
        p.Codigo,
        Historias = p.Historias.Select(h => new
        {
            h.Id,
            h.Capitulo,
            h.TituloCapitulo,
            h.Texto
        })
    });
})
.WithName("GetPersonagemHistoria");

app.MapGet("/api/personagens/raca", async (string codigo, AppDbContext db) =>
{
    var codeLower = codigo.ToLower();
    var p = await db.Personagens
        .AsNoTracking()
        .Include(x => x.Raca)
            .ThenInclude(r => r.TracosRaciais)
        .FirstOrDefaultAsync(x => x.Codigo == codeLower);

    if (p == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    return Results.Ok(new
    {
        p.Id,
        p.Nome,
        p.Codigo,
        Raca = p.Raca.Nome,
        RacaInfo = new
        {
            p.Raca.Nome,
            p.Raca.Descricao,
            p.Raca.TipoCriatura,
            p.Raca.Tamanho,
            p.Raca.Deslocamento
        },
        TracosRaciais = p.Raca.TracosRaciais.Select(t => new
        {
            t.Id,
            t.Nome,
            t.Descricao
        }).ToList()
    });
})
.WithName("GetPersonagemRaca");

app.MapGet("/api/personagens/classe", async (string codigo, AppDbContext db) =>
{
    var codeLower = codigo.ToLower();
    var p = await db.Personagens
        .AsNoTracking()
        .Include(x => x.ClassesPersonagens)
            .ThenInclude(cp => cp.Classe)
                .ThenInclude(c => c.Progressoes)
        .Include(x => x.ClassesPersonagens)
            .ThenInclude(cp => cp.Classe)
                .ThenInclude(c => c.Caracteristicas)
        .Include(x => x.ClassesPersonagens)
            .ThenInclude(cp => cp.Subclasse)
        .FirstOrDefaultAsync(x => x.Codigo == codeLower);

    if (p == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    return Results.Ok(new
    {
        p.Id,
        p.Nome,
        p.Codigo,
        Classes = p.ClassesPersonagens.Select(cp => new
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
        })
    });
})
.WithName("GetPersonagemClasse");

app.MapGet("/api/personagens/ficha", async (string codigo, AppDbContext db) =>
{
    var codeLower = codigo.ToLower();
    var p = await db.Personagens
        .AsNoTracking()
        .AsSplitQuery()
        .Include(x => x.ClassesPersonagens)
            .ThenInclude(cp => cp.Classe)
                .ThenInclude(c => c.Progressoes)
        .Include(x => x.ClassesPersonagens)
            .ThenInclude(cp => cp.Subclasse)
        .Include(x => x.PersonagensPericias)
        .Include(x => x.Raca)
            .ThenInclude(r => r.TracosRaciais)
                .ThenInclude(t => t.TracosRaciaisPericias)
        .Include(x => x.Acoes)
        .Include(x => x.Proficiencias)
        .Include(x => x.Salvaguardas)
        .Include(x => x.PersonagensIdiomas)
            .ThenInclude(pi => pi.Idioma)
        .Include(x => x.PersonagensMagias)
            .ThenInclude(pm => pm.Magia)
        .Include(x => x.Equipamentos)
            .ThenInclude(pe => pe.Equipamento)
        .FirstOrDefaultAsync(x => x.Codigo == codeLower);

    if (p == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    var todasPericias = await db.Pericias.AsNoTracking().OrderBy(per => per.Nome).ToListAsync();
    var nivelTotal = p.ClassesPersonagens.Sum(cp => cp.Nivel);
    var bonusProficiencia = 2 + (nivelTotal - 1) / 4;

    int ObterModificador(int valor) => (int)Math.Floor((valor - 10) / 2.0);

    var mods = new Dictionary<string, int>
    {
        { "Força", ObterModificador(p.Forca) },
        { "Destreza", ObterModificador(p.Destreza) },
        { "Constituição", ObterModificador(p.Constituicao) },
        { "Inteligência", ObterModificador(p.Inteligencia) },
        { "Sabedoria", ObterModificador(p.Sabedoria) },
        { "Carisma", ObterModificador(p.Carisma) }
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
        var isProf = p.Salvaguardas.Any(s => s.Atributo.Equals(dbKey, StringComparison.OrdinalIgnoreCase) && s.IsProficiente);
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

    var classStr = p.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Subclasse != null ? $"{cp.Classe.Nome} / {cp.Subclasse.Nome}" : cp.Classe.Nome).FirstOrDefault() ?? "Sem Classe";

    return Results.Ok(new
    {
        p.Id,
        p.Nome,
        p.Codigo,
        Raca = p.Raca.Nome,
        RacaInfo = new
        {
            p.Raca.Nome,
            p.Raca.Descricao,
            p.Raca.TipoCriatura,
            p.Raca.Tamanho,
            p.Raca.Deslocamento
        },
        TracosRaciais = p.Raca.TracosRaciais.Select(t => new
        {
            t.Id,
            t.Nome,
            t.Descricao
        }).ToList(),
        Classe = p.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Classe.Nome).FirstOrDefault() ?? "Sem Classe",
        Subclasse = p.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Subclasse != null ? cp.Subclasse.Nome : null).FirstOrDefault(),
        Nivel = p.ClassesPersonagens.Sum(cp => cp.Nivel),
        ClassAndSubclass = classStr,
        p.Alinhamento,
        p.Forca,
        p.Destreza,
        p.Constituicao,
        p.Inteligencia,
        p.Sabedoria,
        p.Carisma,
        p.VidaMaxima,
        p.VidaAtual,
        Classes = p.ClassesPersonagens.Select(cp => new
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
                .FirstOrDefault()
        }),
        Pericias = todasPericias.Select(per =>
        {
            var pp = p.PersonagensPericias.FirstOrDefault(x => x.IdPericia == per.Id);
            var isProficienteManual = pp?.IsProficiente ?? false;
            var isMaestriaManual = pp?.IsMaestria ?? false;

            var tracoRacialQueDaPericia = p.Raca.TracosRaciais
                .FirstOrDefault(t => t.TracosRaciaisPericias.Any(trp => trp.IdPericia == per.Id));
            var isProficienteRacial = tracoRacialQueDaPericia != null;
            
            var isMaestriaRacial = tracoRacialQueDaPericia != null && tracoRacialQueDaPericia.TracosRaciaisPericias
                .Any(trp => trp.IdPericia == per.Id && trp.IsMaestria);

            var isProf = isProficienteManual || isProficienteRacial;
            var isMaestria = isMaestriaManual || isMaestriaRacial;

            string? origem = null;
            if (isProficienteRacial)
            {
                origem = $"Raça ({p.Raca.Nome}) - Traço ({tracoRacialQueDaPericia!.Nome})";
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
        Acoes = p.Acoes.Select(a => new
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
        Proficiencias = p.Proficiencias.Select(pr => new
        {
            pr.Id,
            pr.Tipo,
            pr.Nome,
            pr.Origem
        }).ToList(),
        Idiomas = p.PersonagensIdiomas.Select(pi => pi.Idioma.Nome).ToList(),
        Magias = p.PersonagensMagias.Select(pm => new
        {
            pm.Magia.Id,
            pm.Magia.Nome,
            pm.Magia.Nivel,
            pm.Magia.Descricao
        }).ToList(),
        Equipamentos = p.Equipamentos.Select(pe => new
        {
            pe.Id,
            pe.IdEquipamento,
            pe.IsEquipado,
            Equipamento = new
            {
                pe.Equipamento.Id,
                pe.Equipamento.Nome,
                pe.Equipamento.Descricao,
                pe.Equipamento.Peso,
                pe.Equipamento.ProficienciaRequerida,
                pe.Equipamento.TipoEquipamento,
                pe.Equipamento.Propriedades,
                pe.Equipamento.Dano,
                pe.Equipamento.TipoDano,
                pe.Equipamento.ModificadorClasseArmadura,
                pe.Equipamento.ClasseArmadura,
                pe.Equipamento.PermiteDestreza,
                pe.Equipamento.ForcaRequerida,
                pe.Equipamento.DesvantagemFurtividade
            }
        }).ToList()
    });
})
.WithName("GetPersonagemFicha");

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

app.MapPost("/api/personagens/{codigo}/equipamentos/{id}/equipar", async (string codigo, int id, bool confirm, AppDbContext db) =>
{
    var codeLower = codigo.ToLower();
    var character = await db.Personagens
        .Include(p => p.Equipamentos)
            .ThenInclude(pe => pe.Equipamento)
        .Include(p => p.Proficiencias)
        .FirstOrDefaultAsync(p => p.Codigo == codeLower);

    if (character == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    var peToEquip = character.Equipamentos.FirstOrDefault(pe => pe.Id == id);
    if (peToEquip == null)
    {
        return Results.NotFound(new { Message = "Equipamento não encontrado no inventário do personagem." });
    }

    if (peToEquip.Equipamento.TipoEquipamento == "Outro")
    {
        return Results.BadRequest(new { Message = "Este item não é equipável." });
    }

    if (peToEquip.IsEquipado)
    {
        return Results.Ok(new { Message = "O item já está equipado." });
    }

    // Validação de proficiência
    if (!string.IsNullOrEmpty(peToEquip.Equipamento.ProficienciaRequerida))
    {
        var temProf = character.Proficiencias.Any(p => p.Nome.Equals(peToEquip.Equipamento.ProficienciaRequerida, StringComparison.OrdinalIgnoreCase));
        if (!temProf)
        {
            return Results.BadRequest(new { Message = $"Você não tem proficiência para equipar o item {peToEquip.Equipamento.Nome}!" });
        }
    }

    // Regra especial para Armadura
    if (peToEquip.Equipamento.TipoEquipamento == "Armadura")
    {
        var activeArmadura = character.Equipamentos.FirstOrDefault(pe => pe.IsEquipado && pe.Equipamento.TipoEquipamento == "Armadura");
        if (activeArmadura != null)
        {
            if (!confirm)
            {
                return Results.Ok(new { RequiresConfirmation = true, Message = $"Você já possui a armadura {activeArmadura.Equipamento.Nome} equipada. Deseja substituí-la por {peToEquip.Equipamento.Nome}?" });
            }
            else
            {
                activeArmadura.IsEquipado = false;
            }
        }
    }

    // Regra especial para Mãos (Arma ou Escudo)
    if (peToEquip.Equipamento.TipoEquipamento == "Arma" || peToEquip.Equipamento.TipoEquipamento == "Escudo")
    {
        var isTwoHanded = peToEquip.Equipamento.Propriedades.Any(p => p.Equals("Duas mãos", StringComparison.OrdinalIgnoreCase));
        var equippedHandItems = character.Equipamentos
            .Where(pe => pe.IsEquipado && (pe.Equipamento.TipoEquipamento == "Arma" || pe.Equipamento.TipoEquipamento == "Escudo"))
            .ToList();

        if (isTwoHanded)
        {
            // Desequipa tudo nas mãos
            foreach (var item in equippedHandItems)
            {
                item.IsEquipado = false;
            }
        }
        else
        {
            // Se houver alguma arma de duas mãos equipada, desequipa ela
            var twoHanded = equippedHandItems.FirstOrDefault(item => item.Equipamento.Propriedades.Any(p => p.Equals("Duas mãos", StringComparison.OrdinalIgnoreCase)));
            if (twoHanded != null)
            {
                twoHanded.IsEquipado = false;
                equippedHandItems.Remove(twoHanded);
            }

            // Se após remover a de duas mãos ainda temos 2 ou mais itens equipados nas mãos (ex: 2 adagas)
            var currentlyEquipped = equippedHandItems.Where(item => item.IsEquipado).ToList();
            if (currentlyEquipped.Count >= 2)
            {
                // Desequipa o mais antigo (menor Id de PersonagemEquipamento)
                var oldest = currentlyEquipped.OrderBy(item => item.Id).First();
                oldest.IsEquipado = false;
            }
        }
    }

    peToEquip.IsEquipado = true;
    await db.SaveChangesAsync();

    return Results.Ok(new { Message = "Item equipado com sucesso." });
})
.WithName("EquiparEquipamento");

app.MapPost("/api/personagens/{codigo}/equipamentos/{id}/desequipar", async (string codigo, int id, AppDbContext db) =>
{
    var codeLower = codigo.ToLower();
    var character = await db.Personagens
        .Include(p => p.Equipamentos)
        .FirstOrDefaultAsync(p => p.Codigo == codeLower);

    if (character == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    var pe = character.Equipamentos.FirstOrDefault(x => x.Id == id);
    if (pe == null)
    {
        return Results.NotFound(new { Message = "Equipamento não encontrado no inventário do personagem." });
    }

    pe.IsEquipado = false;
    await db.SaveChangesAsync();

    return Results.Ok(new { Message = "Item desequipado com sucesso." });
})
.WithName("DesequiparEquipamento");

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
