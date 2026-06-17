namespace Domain.Entities;

public class HistoriaPersonagem
{
    public int Id { get; set; }
    public int IdPersonagem { get; set; }
    public int Capitulo { get; set; }
    public string? TituloCapitulo { get; set; }
    public required string Texto { get; set; }

    // Navigation property
    public Personagem? Personagem { get; set; }
}
