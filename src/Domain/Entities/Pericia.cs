namespace Domain.Entities;

public class Pericia
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string ModificadorAtributo { get; set; } // Ex: "Força", "Destreza", etc.
}
