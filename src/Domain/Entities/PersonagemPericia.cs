namespace Domain.Entities;

public class PersonagemPericia
{
    public int IdPersonagem { get; set; }
    public Personagem Personagem { get; set; } = null!;

    public int IdPericia { get; set; }
    public Pericia Pericia { get; set; } = null!;

    public bool IsProficiente { get; set; }
    public bool IsMaestria { get; set; }
    public required string Origem { get; set; }
}
