using System.Collections.Generic;

namespace Domain.Entities;

public class Equipamento
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public string? Descricao { get; set; }
    public double? Peso { get; set; }
    public string? ProficienciaRequerida { get; set; }
    public string TipoEquipamento { get; set; } = "Outro"; // Arma, Armadura, Escudo, Outro
    public List<string> Propriedades { get; set; } = new List<string>();

    // Específicos de Arma
    public string? Dano { get; set; } // Ex: 1d4, 1d8
    public string? TipoDano { get; set; } // Ex: perfurante, concussão

    // Específicos de Escudo / Armadura
    public int? ModificadorClasseArmadura { get; set; } // Adicional ao CA (ex: +2 para escudo)

    // Específicos de Armadura
    public int? ClasseArmadura { get; set; } // CA base (ex: 12)
    public bool? PermiteDestreza { get; set; } // Se permite adicionar mod de des
    public int? ForcaRequerida { get; set; } // Força mínima (ex: 13, 15)
    public bool? DesvantagemFurtividade { get; set; } // Se causa desvantagem em furtividade
}
