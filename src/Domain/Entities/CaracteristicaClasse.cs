namespace Domain.Entities;

public class CaracteristicaClasse
{
    public int Id { get; set; }
    public int IdClasse { get; set; }
    public Classe Classe { get; set; } = null!;
    public int Nivel { get; set; }
    public required string Nome { get; set; }
    public required string Descricao { get; set; }
}
