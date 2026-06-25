using System.Collections.Generic;

namespace Domain.Entities;

public class Personagem
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Codigo { get; set; }
    public string? Base64Imagem { get; set; }
    public int IdRaca { get; set; }
    public Raca Raca { get; set; } = null!;
    public string? Alinhamento { get; set; }

    // Atributos base D&D
    public int Forca { get; set; }
    public int Destreza { get; set; }
    public int Constituicao { get; set; }
    public int Inteligencia { get; set; }
    public int Sabedoria { get; set; }
    public int Carisma { get; set; }

    // Pontos de Vida persistentes
    public int VidaMaxima { get; set; }
    public int VidaAtual { get; set; }

    // Dinheiro / Moedas D&D
    public int PecaCobre { get; set; }
    public int PecaPrata { get; set; }
    public int PecaElectro { get; set; }
    public int PecaOuro { get; set; }
    public int PecaPlatina { get; set; }

    // Navigation properties
    public ICollection<HistoriaPersonagem> Historias { get; set; } = new List<HistoriaPersonagem>();
    public ICollection<ClassePersonagem> ClassesPersonagens { get; set; } = new List<ClassePersonagem>();
    public ICollection<PersonagemPericia> PersonagensPericias { get; set; } = new List<PersonagemPericia>();
    public ICollection<AcaoPersonagem> Acoes { get; set; } = new List<AcaoPersonagem>();
    public ICollection<PersonagemProficiencia> Proficiencias { get; set; } = new List<PersonagemProficiencia>();
    public ICollection<PersonagemSalvaguarda> Salvaguardas { get; set; } = new List<PersonagemSalvaguarda>();
    public ICollection<PersonagemIdioma> PersonagensIdiomas { get; set; } = new List<PersonagemIdioma>();
    public ICollection<PersonagemMagia> PersonagensMagias { get; set; } = new List<PersonagemMagia>();
    public ICollection<PersonagemEquipamento> Equipamentos { get; set; } = new List<PersonagemEquipamento>();
}
