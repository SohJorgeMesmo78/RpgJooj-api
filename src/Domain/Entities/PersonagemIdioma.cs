namespace Domain.Entities;

public class PersonagemIdioma
{
    public int IdPersonagem { get; set; }
    public Personagem Personagem { get; set; } = null!;

    public int IdIdioma { get; set; }
    public Idioma Idioma { get; set; } = null!;
}
