namespace Domain.Entities;

public class PersonagemSalvaguarda
{
    public int IdPersonagem { get; set; }
    public Personagem Personagem { get; set; } = null!;
    public required string Atributo { get; set; } // Ex: "Forca", "Destreza", "Constituicao", "Inteligencia", "Sabedoria", "Carisma"
    public bool IsProficiente { get; set; }
}
