namespace Domain.Entities;

public class PersonagemMagia
{
    public int IdPersonagem { get; set; }
    public Personagem Personagem { get; set; } = null!;

    public int IdMagia { get; set; }
    public Magia Magia { get; set; } = null!;
}
