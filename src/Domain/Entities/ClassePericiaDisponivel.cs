namespace Domain.Entities;

public class ClassePericiaDisponivel
{
    public int IdClasse { get; set; }
    public Classe Classe { get; set; } = null!;

    public int IdPericia { get; set; }
    public Pericia Pericia { get; set; } = null!;
}
