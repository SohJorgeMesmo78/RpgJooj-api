using System.Collections.Generic;

namespace Domain.Entities;

public class Classe
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public string? Subclasse { get; set; }
    public required string DadoVida { get; set; } // Ex: "d8", "d10"
    public int? Deslocamento { get; set; } // Opcional, ex: monge ganha bônus no lvl 2
    public int QtdPericiasEscolha { get; set; }

    // Navigation properties
    public ICollection<ClassePersonagem> ClassesPersonagens { get; set; } = new List<ClassePersonagem>();
    public ICollection<ClassePericiaDisponivel> PericiasDisponiveis { get; set; } = new List<ClassePericiaDisponivel>();
}
