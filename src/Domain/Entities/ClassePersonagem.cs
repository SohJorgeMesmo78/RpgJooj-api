namespace Domain.Entities;

public class ClassePersonagem
{
    public int IdPersonagem { get; set; }
    public Personagem Personagem { get; set; } = null!;

    public int IdClasse { get; set; }
    public Classe Classe { get; set; } = null!;

    public int Nivel { get; set; }
}
