using System.Collections.Generic;

namespace Domain.Entities;

public class Raca
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Descricao { get; set; }
    public required string TipoCriatura { get; set; }
    public required string Tamanho { get; set; }
    public int Deslocamento { get; set; } // Em metros

    // Navigation properties
    public ICollection<Personagem> Personagens { get; set; } = new List<Personagem>();
    public ICollection<TracoRacial> TracosRaciais { get; set; } = new List<TracoRacial>();
}
