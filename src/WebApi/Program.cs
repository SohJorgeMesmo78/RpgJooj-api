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
        .Select(p => new
        {
            p.Id,
            p.Nome,
            p.Codigo,
            p.Raca,
            Classe = p.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Classe.Nome).FirstOrDefault() ?? "Sem Classe",
            Subclasse = p.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Classe.Subclasse).FirstOrDefault(),
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
        .FirstOrDefaultAsync(p => p.Codigo.ToLower() == codigo.ToLower());

    if (personagem == null)
    {
        return Results.NotFound(new { Message = $"Personagem com código '{codigo}' não encontrado." });
    }

    return Results.Ok(new
    {
        personagem.Id,
        personagem.Nome,
        personagem.Codigo,
        personagem.Base64Imagem,
        personagem.Raca,
        Classe = personagem.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Classe.Nome).FirstOrDefault() ?? "Sem Classe",
        Subclasse = personagem.ClassesPersonagens.OrderBy(cp => cp.IdClasse).Select(cp => cp.Classe.Subclasse).FirstOrDefault(),
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
            cp.Classe.Subclasse,
            cp.Classe.DadoVida,
            cp.Classe.Deslocamento,
            cp.Nivel
        }),
        Historias = personagem.Historias.Select(h => new
        {
            h.Id,
            h.Capitulo,
            h.TituloCapitulo,
            h.Texto
        })
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

    var personagem = new Personagem
    {
        Nome = dto.Nome,
        Codigo = dto.Codigo,
        Base64Imagem = dto.Base64Imagem,
        Raca = dto.Raca,
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
    var classeDb = await db.Classes.FirstOrDefaultAsync(c => c.Nome.ToLower() == dto.Classe.ToLower() && (c.Subclasse == null || c.Subclasse.ToLower() == dto.Subclasse.ToLower()));
    if (classeDb == null)
    {
        classeDb = new Classe
        {
            Nome = dto.Classe,
            Subclasse = dto.Subclasse,
            DadoVida = "d8",
            Deslocamento = 9
        };
        db.Classes.Add(classeDb);
        await db.SaveChangesAsync();
    }

    var cp = new ClassePersonagem
    {
        IdPersonagem = personagem.Id,
        IdClasse = classeDb.Id,
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
        personagem.Raca,
        Classe = classeDb.Nome,
        Subclasse = classeDb.Subclasse,
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
