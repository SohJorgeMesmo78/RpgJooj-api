using System.Collections.Generic;

namespace Domain.Entities;

public class Pericia
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string ModificadorAtributo { get; set; } // Ex: "Força", "Destreza", etc.

    // Navigation properties
    public ICollection<PersonagemPericia> PersonagensPericias { get; set; } = new List<PersonagemPericia>();
    public ICollection<TracoRacialPericia> TracosRaciaisPericias { get; set; } = new List<TracoRacialPericia>();
    public ICollection<ClassePericiaDisponivel> ClassesPericiasDisponiveis { get; set; } = new List<ClassePericiaDisponivel>();
}
