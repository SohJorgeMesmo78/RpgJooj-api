namespace Domain.Entities;

public class PersonagemProficiencia
{
    public int Id { get; set; }
    public int IdPersonagem { get; set; }
    public Personagem Personagem { get; set; } = null!;
    public required string Tipo { get; set; } // Ex: "Armadura", "Arma", "Ferramenta"
    public required string Nome { get; set; }
    public required string Origem { get; set; }
}
