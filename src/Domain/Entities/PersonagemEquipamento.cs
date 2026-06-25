namespace Domain.Entities;

public class PersonagemEquipamento
{
    public int Id { get; set; }
    
    public int IdPersonagem { get; set; }
    public Personagem Personagem { get; set; } = null!;
    
    public int IdEquipamento { get; set; }
    public Equipamento Equipamento { get; set; } = null!;
    
    public bool IsEquipado { get; set; }
}
