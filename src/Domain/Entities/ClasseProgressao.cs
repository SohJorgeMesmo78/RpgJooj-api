namespace Domain.Entities;

public class ClasseProgressao
{
    public int Id { get; set; }
    public int IdClasse { get; set; }
    public Classe Classe { get; set; } = null!;
    
    public int Nivel { get; set; }
    public int BonusProficiencia { get; set; }
    public int TruquesConhecidos { get; set; }
    public int MagiasConhecidas { get; set; }
    public int EspacosMagia { get; set; }
    public int NivelMagia { get; set; }
    public int InvocacoesConhecidas { get; set; }
}
