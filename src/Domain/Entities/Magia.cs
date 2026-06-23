using System.Collections.Generic;

namespace Domain.Entities;

public class Magia
{
    public int Id { get; set; }
    public int Nivel { get; set; } // 0 = Truque / Cantrip
    public required string Nome { get; set; }
    public required string Descricao { get; set; }

    // Navigation properties
    public ICollection<PersonagemMagia> PersonagensMagias { get; set; } = new List<PersonagemMagia>();
}
