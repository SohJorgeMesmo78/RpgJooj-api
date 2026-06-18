using System.Collections.Generic;

namespace Domain.Entities;

public class Idioma
{
    public int Id { get; set; }
    public required string Nome { get; set; }

    // Navigation property
    public ICollection<PersonagemIdioma> PersonagensIdiomas { get; set; } = new List<PersonagemIdioma>();
}
