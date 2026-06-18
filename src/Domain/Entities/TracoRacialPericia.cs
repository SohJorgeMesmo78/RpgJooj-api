namespace Domain.Entities;

public class TracoRacialPericia
{
    public int IdTracoRacial { get; set; }
    public TracoRacial TracoRacial { get; set; } = null!;

    public int IdPericia { get; set; }
    public Pericia Pericia { get; set; } = null!;

    public bool IsMaestria { get; set; }
}
