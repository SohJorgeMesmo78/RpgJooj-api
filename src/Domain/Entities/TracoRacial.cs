using System.Collections.Generic;

namespace Domain.Entities;

public class TracoRacial
{
    public int Id { get; set; }
    public int IdRaca { get; set; }
    public Raca Raca { get; set; } = null!;
    public required string Nome { get; set; }
    public required string Descricao { get; set; }

    public ICollection<TracoRacialPericia> TracosRaciaisPericias { get; set; } = new List<TracoRacialPericia>();
}
