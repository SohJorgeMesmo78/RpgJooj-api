namespace Domain.Entities;

public class AcaoPersonagem
{
    public int Id { get; set; }
    public int IdPersonagem { get; set; }
    public Personagem Personagem { get; set; } = null!;
    public required string Nome { get; set; }
    public required string TipoAcao { get; set; } // Ex: "Ação", "Ação Bônus", "Reação"
    public required string Alcance { get; set; }
    public required string BonusAcerto { get; set; }
    public required string Dano { get; set; }
    public required string TipoDano { get; set; }
    public string? Descricao { get; set; }
}
