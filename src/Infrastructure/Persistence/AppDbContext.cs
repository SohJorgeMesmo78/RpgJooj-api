using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Personagem> Personagens => Set<Personagem>();
    public DbSet<HistoriaPersonagem> HistoriasPersonagens => Set<HistoriaPersonagem>();
    public DbSet<Classe> Classes => Set<Classe>();
    public DbSet<ClassePersonagem> ClassesPersonagens => Set<ClassePersonagem>();
    public DbSet<Pericia> Pericias => Set<Pericia>();
    public DbSet<Raca> Racas => Set<Raca>();
    public DbSet<TracoRacial> TracosRaciais => Set<TracoRacial>();
    public DbSet<PersonagemPericia> PersonagensPericias => Set<PersonagemPericia>();
    public DbSet<TracoRacialPericia> TracosRaciaisPericias => Set<TracoRacialPericia>();
    public DbSet<AcaoPersonagem> AcoesPersonagens => Set<AcaoPersonagem>();
    public DbSet<PersonagemProficiencia> PersonagensProficiencias => Set<PersonagemProficiencia>();
    public DbSet<PersonagemSalvaguarda> PersonagensSalvaguardas => Set<PersonagemSalvaguarda>();
    public DbSet<ClassePericiaDisponivel> ClassesPericiasDisponiveis => Set<ClassePericiaDisponivel>();
    public DbSet<Idioma> Idiomas => Set<Idioma>();
    public DbSet<PersonagemIdioma> PersonagensIdiomas => Set<PersonagemIdioma>();
    public DbSet<ClasseProgressao> ClasseProgressoes => Set<ClasseProgressao>();
    public DbSet<Magia> Magias => Set<Magia>();
    public DbSet<PersonagemMagia> PersonagensMagias => Set<PersonagemMagia>();
    public DbSet<CaracteristicaClasse> CaracteristicasClasses => Set<CaracteristicaClasse>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Personagem entity
        modelBuilder.Entity<Personagem>(entity =>
        {
            entity.ToTable("Personagens");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Codigo).IsRequired().HasMaxLength(100);
            entity.Property(p => p.IdRaca).IsRequired();
            entity.Property(p => p.Alinhamento).HasMaxLength(100);
            
            // Relationship configuration
            entity.HasOne(p => p.Raca)
                .WithMany(r => r.Personagens)
                .HasForeignKey(p => p.IdRaca)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(p => p.Forca).IsRequired();
            entity.Property(p => p.Destreza).IsRequired();
            entity.Property(p => p.Constituicao).IsRequired();
            entity.Property(p => p.Inteligencia).IsRequired();
            entity.Property(p => p.Sabedoria).IsRequired();
            entity.Property(p => p.Carisma).IsRequired();
            entity.Property(p => p.VidaMaxima).IsRequired();
            entity.Property(p => p.VidaAtual).IsRequired();
            entity.HasIndex(p => p.Codigo).IsUnique();
        });

        // Configure HistoriaPersonagem entity
        modelBuilder.Entity<HistoriaPersonagem>(entity =>
        {
            entity.ToTable("HistoriasPersonagens");
            entity.HasKey(hp => hp.Id);
            entity.Property(hp => hp.Texto).IsRequired();
            entity.Property(hp => hp.TituloCapitulo).HasMaxLength(200);

            // Relationship configuration
            entity.HasOne(hp => hp.Personagem)
                .WithMany(p => p.Historias)
                .HasForeignKey(hp => hp.IdPersonagem)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Classe entity
        modelBuilder.Entity<Classe>(entity =>
        {
            entity.ToTable("Classes");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nome).IsRequired().HasMaxLength(100);
            entity.Property(c => c.DadoVida).IsRequired().HasMaxLength(10);
            entity.Property(c => c.Deslocamento).IsRequired(false);
            entity.Property(c => c.IdClassePai).IsRequired(false);

            entity.HasOne(c => c.ClassePai)
                .WithMany(c => c.Subclasses)
                .HasForeignKey(c => c.IdClassePai)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ClassePersonagem entity
        modelBuilder.Entity<ClassePersonagem>(entity =>
        {
            entity.ToTable("ClassesPersonagens");
            entity.HasKey(cp => new { cp.IdPersonagem, cp.IdClasse });

            entity.HasOne(cp => cp.Personagem)
                .WithMany(p => p.ClassesPersonagens)
                .HasForeignKey(cp => cp.IdPersonagem)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cp => cp.Classe)
                .WithMany(c => c.ClassesPersonagens)
                .HasForeignKey(cp => cp.IdClasse)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(cp => cp.IdSubclasse).IsRequired(false);
            entity.HasOne(cp => cp.Subclasse)
                .WithMany()
                .HasForeignKey(cp => cp.IdSubclasse)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(cp => cp.Nivel).IsRequired();
        });

        // Configure Pericia entity
        modelBuilder.Entity<Pericia>(entity =>
        {
            entity.ToTable("Pericias");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            entity.Property(p => p.ModificadorAtributo).IsRequired().HasMaxLength(50);
        });

        // Configure Raca entity
        modelBuilder.Entity<Raca>(entity =>
        {
            entity.ToTable("Racas");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Nome).IsRequired().HasMaxLength(100);
            entity.Property(r => r.Descricao).IsRequired();
            entity.Property(r => r.TipoCriatura).IsRequired().HasMaxLength(150);
            entity.Property(r => r.Tamanho).IsRequired().HasMaxLength(50);
            entity.Property(r => r.Deslocamento).IsRequired();
        });

        // Configure TracoRacial entity
        modelBuilder.Entity<TracoRacial>(entity =>
        {
            entity.ToTable("TracosRaciais");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Nome).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Descricao).IsRequired();

            entity.HasOne(t => t.Raca)
                .WithMany(r => r.TracosRaciais)
                .HasForeignKey(t => t.IdRaca)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure PersonagemPericia entity
        modelBuilder.Entity<PersonagemPericia>(entity =>
        {
            entity.ToTable("PersonagensPericias");
            entity.HasKey(pp => new { pp.IdPersonagem, pp.IdPericia });
            entity.Property(pp => pp.Origem).IsRequired().HasMaxLength(150);

            entity.HasOne(pp => pp.Personagem)
                .WithMany(p => p.PersonagensPericias)
                .HasForeignKey(pp => pp.IdPersonagem)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pp => pp.Pericia)
                .WithMany(p => p.PersonagensPericias)
                .HasForeignKey(pp => pp.IdPericia)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure TracoRacialPericia entity
        modelBuilder.Entity<TracoRacialPericia>(entity =>
        {
            entity.ToTable("TracosRaciaisPericias");
            entity.HasKey(trp => new { trp.IdTracoRacial, trp.IdPericia });

            entity.HasOne(trp => trp.TracoRacial)
                .WithMany(t => t.TracosRaciaisPericias)
                .HasForeignKey(trp => trp.IdTracoRacial)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(trp => trp.Pericia)
                .WithMany(p => p.TracosRaciaisPericias)
                .HasForeignKey(trp => trp.IdPericia)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure AcaoPersonagem entity
        modelBuilder.Entity<AcaoPersonagem>(entity =>
        {
            entity.ToTable("AcoesPersonagens");
            entity.HasKey(ap => ap.Id);
            entity.Property(ap => ap.Nome).IsRequired().HasMaxLength(100);
            entity.Property(ap => ap.TipoAcao).IsRequired().HasMaxLength(50);
            entity.Property(ap => ap.Alcance).IsRequired().HasMaxLength(50);
            entity.Property(ap => ap.BonusAcerto).IsRequired().HasMaxLength(50);
            entity.Property(ap => ap.Dano).IsRequired().HasMaxLength(50);
            entity.Property(ap => ap.TipoDano).IsRequired().HasMaxLength(50);
            entity.Property(ap => ap.Descricao).HasMaxLength(500);
            entity.Property(ap => ap.AcertoTooltip).HasMaxLength(150);
            entity.Property(ap => ap.DanoTooltip).HasMaxLength(150);

            entity.HasOne(ap => ap.Personagem)
                .WithMany(p => p.Acoes)
                .HasForeignKey(ap => ap.IdPersonagem)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure PersonagemProficiencia entity
        modelBuilder.Entity<PersonagemProficiencia>(entity =>
        {
            entity.ToTable("PersonagensProficiencias");
            entity.HasKey(pp => pp.Id);
            entity.Property(pp => pp.Tipo).IsRequired().HasMaxLength(50);
            entity.Property(pp => pp.Nome).IsRequired().HasMaxLength(150);
            entity.Property(pp => pp.Origem).IsRequired().HasMaxLength(150);

            entity.HasOne(pp => pp.Personagem)
                .WithMany(p => p.Proficiencias)
                .HasForeignKey(pp => pp.IdPersonagem)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure PersonagemSalvaguarda entity
        modelBuilder.Entity<PersonagemSalvaguarda>(entity =>
        {
            entity.ToTable("PersonagensSalvaguardas");
            entity.HasKey(ps => new { ps.IdPersonagem, ps.Atributo });
            entity.Property(ps => ps.Atributo).IsRequired().HasMaxLength(50);

            entity.HasOne(ps => ps.Personagem)
                .WithMany(p => p.Salvaguardas)
                .HasForeignKey(ps => ps.IdPersonagem)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ClassePericiaDisponivel entity
        modelBuilder.Entity<ClassePericiaDisponivel>(entity =>
        {
            entity.ToTable("ClassesPericiasDisponiveis");
            entity.HasKey(cpd => new { cpd.IdClasse, cpd.IdPericia });

            entity.HasOne(cpd => cpd.Classe)
                .WithMany(c => c.PericiasDisponiveis)
                .HasForeignKey(cpd => cpd.IdClasse)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cpd => cpd.Pericia)
                .WithMany(p => p.ClassesPericiasDisponiveis)
                .HasForeignKey(cpd => cpd.IdPericia)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Idioma entity
        modelBuilder.Entity<Idioma>(entity =>
        {
            entity.ToTable("Idiomas");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Nome).IsRequired().HasMaxLength(100);
        });

        // Configure PersonagemIdioma entity
        modelBuilder.Entity<PersonagemIdioma>(entity =>
        {
            entity.ToTable("PersonagensIdiomas");
            entity.HasKey(pi => new { pi.IdPersonagem, pi.IdIdioma });

            entity.HasOne(pi => pi.Personagem)
                .WithMany(p => p.PersonagensIdiomas)
                .HasForeignKey(pi => pi.IdPersonagem)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pi => pi.Idioma)
                .WithMany(i => i.PersonagensIdiomas)
                .HasForeignKey(pi => pi.IdIdioma)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ClasseProgressao entity
        modelBuilder.Entity<ClasseProgressao>(entity =>
        {
            entity.ToTable("ClasseProgressoes");
            entity.HasKey(cp => cp.Id);
            entity.HasOne(cp => cp.Classe)
                .WithMany(c => c.Progressoes)
                .HasForeignKey(cp => cp.IdClasse)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Magia entity
        modelBuilder.Entity<Magia>(entity =>
        {
            entity.ToTable("Magias");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Nome).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Descricao).IsRequired();
        });

        // Configure PersonagemMagia entity
        modelBuilder.Entity<PersonagemMagia>(entity =>
        {
            entity.ToTable("PersonagensMagias");
            entity.HasKey(pm => new { pm.IdPersonagem, pm.IdMagia });

            entity.HasOne(pm => pm.Personagem)
                .WithMany(p => p.PersonagensMagias)
                .HasForeignKey(pm => pm.IdPersonagem)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pm => pm.Magia)
                .WithMany(m => m.PersonagensMagias)
                .HasForeignKey(pm => pm.IdMagia)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure CaracteristicaClasse entity
        modelBuilder.Entity<CaracteristicaClasse>(entity =>
        {
            entity.ToTable("CaracteristicasClasses");
            entity.HasKey(cc => cc.Id);
            entity.Property(cc => cc.Nome).IsRequired().HasMaxLength(150);
            entity.Property(cc => cc.Descricao).IsRequired();

            entity.HasOne(cc => cc.Classe)
                .WithMany(c => c.Caracteristicas)
                .HasForeignKey(cc => cc.IdClasse)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seeding Classes
        modelBuilder.Entity<Classe>().HasData(
            new Classe { Id = 1, Nome = "Bruxo", IdClassePai = null, DadoVida = "d8", Deslocamento = null, QtdPericiasEscolha = 2 },
            new Classe { Id = 2, Nome = "Monge", IdClassePai = null, DadoVida = "d8", Deslocamento = null, QtdPericiasEscolha = 2 },
            new Classe { Id = 3, Nome = "o Gênio", IdClassePai = 1, DadoVida = "", Deslocamento = null, QtdPericiasEscolha = 0 },
            new Classe { Id = 4, Nome = "Caminho do eu astral", IdClassePai = 2, DadoVida = "", Deslocamento = null, QtdPericiasEscolha = 0 }
        );

        // Seeding Racas
        modelBuilder.Entity<Raca>().HasData(
            new Raca
            {
                Id = 1,
                Nome = "Eladrin",
                Descricao = "Eladrin são elfos da Feywild, um reino de beleza perigosa e magia sem limites. Usando essa magia, eladrin pode ir de um lugar para outro em um piscar de olhos, e cada eladrin ressoa com emoções capturadas na feywild na forma de estações – afinidades que afetam o humor e a aparência do eladrin. A estação de um eladrin pode mudar, embora alguns permaneçam em uma estação para sempre. escolha sua estação ou role na tabela de estações eladrin. Seu traço de Transe permite que você mude sua estação. Como outros elfos, os eladrin podem viver mais de 750 anos.",
                TipoCriatura = "Humanóide. Você também é considerado um elfo para qualquer pré-requisito ou efeito que exija que você seja um elfo.",
                Tamanho = "Médio",
                Deslocamento = 9
            }
        );

        // Seeding TracosRaciais
        modelBuilder.Entity<TracoRacial>().HasData(
            new TracoRacial
            {
                Id = 1,
                IdRaca = 1,
                Nome = "Visão no escuro",
                Descricao = "Você pode ver na penumbra a até 18 metros(60ft) de você como se fosse luz brilhante, e na escuridão como se fosse luz fraca. Você não pode discernir cores na escuridão, apenas tons de cinza."
            },
            new TracoRacial
            {
                Id = 2,
                IdRaca = 1,
                Nome = "Ancestralidade Feérica",
                Descricao = "Você tem vantagem nos testes de resistência que fizer para evitar ou acabar com a condição de encantado em si mesmo."
            },
            new TracoRacial
            {
                Id = 3,
                IdRaca = 1,
                Nome = "Passo Feérico",
                Descricao = "Como ação bônus, você pode se teletransportar magicamente até 9 metros(30ft) para um espaço desocupado que você possa ver. Você pode usar esse traço um número de vezes igual ao seu bônus de proficiência, e você recupera todos os usos gastos quando terminar um descanso longo. Quando você alcança o 3° nível, seu Passo Feérico ganha um efeito adicional baseado em sua estação; se o efeito exigir um teste de resistência, a CD é igual a 8 + seu bônus de proficiência + seu modificador de Inteligência, Sabedoria ou Carisma (escolha ao selecionar esta raça):\n\nOutono. Imediatamente após você usar seu Passo Feérico, até duas criaturas de sua escolha que você possa ver a até 3 metros(10ft) de você devem ser bem sucedidas em um teste de resistência de Sabedoria ou serão enfeitiçadas por você por 1 minuto, ou até que você ou seus companheiros causem qualquer dano às criaturas.\n\nInverno. Quando você usa seu Passo Feérico, uma criatura de sua escolha que você possa ver a 1,5 metro(5ft) de você antes de se teletransportar deve fazer um teste de resistência de Sabedoria ou ficará com medo de você até o final do seu próximo turno.\n\nPrimavera. Quando você usa seu Passo Feérico, você pode tocar uma criatura voluntária a até 1,5 metros(5ft) de você. Essa criatura então se teletransporta em vez de você, aparecendo em um espaço desocupado de sua escolha que você pode ver a até 9 metros(30ft) de você.\n\nVerão. Imediatamente após você usar seu Passo Feérico, cada criatura de sua escolha que você possa ver a até 1,5 metros(5ft) de você sofre dano de fogo igual ao seu bônus de proficiência."
            },
            new TracoRacial
            {
                Id = 4,
                IdRaca = 1,
                Nome = "Sentidos Aguçados",
                Descricao = "Você tem proficiência na perícia Percepção."
            },
            new TracoRacial
            {
                Id = 5,
                IdRaca = 1,
                Nome = "Transe",
                Descricao = "Você não precisa dormir, e magia não pode fazer você dormir. Você pode terminar um descanso longo em 4 horas se passar essas horas in uma meditação como um transe, durante a qual você retém consciência. Sempre que você terminar este transe, você pode mudar sua temporada, e você pode ganhar duas proficiências que você não possui, cada uma com uma arma ou ferramenta de sua escolha selecionada no Livro do Jogador. Você adquire misticamente essas proficiências extraindo-as da memória élfica compartilhada e as mantém até terminar um descanso longo."
            }
        );

        // Seeding Character: Kairo (Id = 1)
        modelBuilder.Entity<Personagem>().HasData(
            new Personagem
            {
                Id = 1,
                Nome = "Kairo",
                Codigo = "kairo",
                Base64Imagem = null,
                IdRaca = 1, // Eladrin
                Alinhamento = "Caótico e Bom",
                Forca = 8,
                Destreza = 16,
                Constituicao = 13,
                Inteligencia = 13,
                Sabedoria = 12,
                Carisma = 19,
                VidaMaxima = 9,
                VidaAtual = 9
            }
        );

        // Seeding ClassesPersonagens
        modelBuilder.Entity<ClassePersonagem>().HasData(
            new ClassePersonagem { IdPersonagem = 1, IdClasse = 1, IdSubclasse = 3, Nivel = 1 }
        );

        // Seeding ClasseProgressoes
        modelBuilder.Entity<ClasseProgressao>().HasData(
            new ClasseProgressao { Id = 1, IdClasse = 1, Nivel = 1, BonusProficiencia = 2, TruquesConhecidos = 2, MagiasConhecidas = 2, EspacosMagia = 1, NivelMagia = 1, InvocacoesConhecidas = 0 },
            new ClasseProgressao { Id = 2, IdClasse = 1, Nivel = 2, BonusProficiencia = 2, TruquesConhecidos = 2, MagiasConhecidas = 3, EspacosMagia = 2, NivelMagia = 1, InvocacoesConhecidas = 2 },
            new ClasseProgressao { Id = 3, IdClasse = 1, Nivel = 3, BonusProficiencia = 2, TruquesConhecidos = 2, MagiasConhecidas = 4, EspacosMagia = 2, NivelMagia = 2, InvocacoesConhecidas = 2 },
            new ClasseProgressao { Id = 4, IdClasse = 1, Nivel = 4, BonusProficiencia = 2, TruquesConhecidos = 3, MagiasConhecidas = 5, EspacosMagia = 2, NivelMagia = 2, InvocacoesConhecidas = 3 },
            new ClasseProgressao { Id = 5, IdClasse = 1, Nivel = 5, BonusProficiencia = 3, TruquesConhecidos = 3, MagiasConhecidas = 6, EspacosMagia = 2, NivelMagia = 3, InvocacoesConhecidas = 3 },
            new ClasseProgressao { Id = 6, IdClasse = 1, Nivel = 6, BonusProficiencia = 3, TruquesConhecidos = 3, MagiasConhecidas = 7, EspacosMagia = 2, NivelMagia = 3, InvocacoesConhecidas = 4 },
            new ClasseProgressao { Id = 7, IdClasse = 1, Nivel = 7, BonusProficiencia = 3, TruquesConhecidos = 3, MagiasConhecidas = 8, EspacosMagia = 2, NivelMagia = 4, InvocacoesConhecidas = 4 },
            new ClasseProgressao { Id = 8, IdClasse = 1, Nivel = 8, BonusProficiencia = 3, TruquesConhecidos = 3, MagiasConhecidas = 9, EspacosMagia = 2, NivelMagia = 4, InvocacoesConhecidas = 4 },
            new ClasseProgressao { Id = 9, IdClasse = 1, Nivel = 9, BonusProficiencia = 4, TruquesConhecidos = 3, MagiasConhecidas = 10, EspacosMagia = 2, NivelMagia = 5, InvocacoesConhecidas = 5 },
            new ClasseProgressao { Id = 10, IdClasse = 1, Nivel = 10, BonusProficiencia = 4, TruquesConhecidos = 4, MagiasConhecidas = 10, EspacosMagia = 2, NivelMagia = 5, InvocacoesConhecidas = 5 },
            new ClasseProgressao { Id = 11, IdClasse = 1, Nivel = 11, BonusProficiencia = 4, TruquesConhecidos = 4, MagiasConhecidas = 11, EspacosMagia = 3, NivelMagia = 5, InvocacoesConhecidas = 5 },
            new ClasseProgressao { Id = 12, IdClasse = 1, Nivel = 12, BonusProficiencia = 4, TruquesConhecidos = 4, MagiasConhecidas = 11, EspacosMagia = 3, NivelMagia = 5, InvocacoesConhecidas = 6 },
            new ClasseProgressao { Id = 13, IdClasse = 1, Nivel = 13, BonusProficiencia = 5, TruquesConhecidos = 4, MagiasConhecidas = 12, EspacosMagia = 3, NivelMagia = 5, InvocacoesConhecidas = 6 },
            new ClasseProgressao { Id = 14, IdClasse = 1, Nivel = 14, BonusProficiencia = 5, TruquesConhecidos = 4, MagiasConhecidas = 12, EspacosMagia = 3, NivelMagia = 5, InvocacoesConhecidas = 6 },
            new ClasseProgressao { Id = 15, IdClasse = 1, Nivel = 15, BonusProficiencia = 5, TruquesConhecidos = 4, MagiasConhecidas = 13, EspacosMagia = 3, NivelMagia = 5, InvocacoesConhecidas = 7 },
            new ClasseProgressao { Id = 16, IdClasse = 1, Nivel = 16, BonusProficiencia = 5, TruquesConhecidos = 4, MagiasConhecidas = 13, EspacosMagia = 3, NivelMagia = 5, InvocacoesConhecidas = 7 },
            new ClasseProgressao { Id = 17, IdClasse = 1, Nivel = 17, BonusProficiencia = 6, TruquesConhecidos = 4, MagiasConhecidas = 14, EspacosMagia = 4, NivelMagia = 5, InvocacoesConhecidas = 7 },
            new ClasseProgressao { Id = 18, IdClasse = 1, Nivel = 18, BonusProficiencia = 6, TruquesConhecidos = 4, MagiasConhecidas = 14, EspacosMagia = 4, NivelMagia = 5, InvocacoesConhecidas = 8 },
            new ClasseProgressao { Id = 19, IdClasse = 1, Nivel = 19, BonusProficiencia = 6, TruquesConhecidos = 4, MagiasConhecidas = 15, EspacosMagia = 4, NivelMagia = 5, InvocacoesConhecidas = 8 },
            new ClasseProgressao { Id = 20, IdClasse = 1, Nivel = 20, BonusProficiencia = 6, TruquesConhecidos = 4, MagiasConhecidas = 15, EspacosMagia = 4, NivelMagia = 5, InvocacoesConhecidas = 8 }
        );

        // Seeding CaracteristicasClasses
        modelBuilder.Entity<CaracteristicaClasse>().HasData(
            new CaracteristicaClasse { Id = 1, IdClasse = 1, Nivel = 1, Nome = "Patrono Transcendental", Descricao = "Seu patrono concede a você características no 1º nível e novamente no 6º, 10º e 14º nível." },
            new CaracteristicaClasse { Id = 2, IdClasse = 1, Nivel = 1, Nome = "Magia de Pacto", Descricao = "Sua pesquisa arcana e a magia infundida em você por seu patrono concedem a você a capacidade de conjurar magias." },
            new CaracteristicaClasse { Id = 3, IdClasse = 1, Nivel = 2, Nome = "Invocações Místicas", Descricao = "Em suas investigações sobre o conhecimento oculto, você descobriu invocações místicas, fragmentos de conhecimento proibido que infundem você com uma habilidade mágica duradoura." },
            new CaracteristicaClasse { Id = 4, IdClasse = 1, Nivel = 3, Nome = "Dádiva do Pacto", Descricao = "No 3º nível, seu patrono transcendental concede a você um favor por seus serviços leais. Você ganha uma das características: Pacto da Corrente, Pacto da Lâmina ou Pacto do Tomo." },
            new CaracteristicaClasse { Id = 5, IdClasse = 1, Nivel = 4, Nome = "Incremento no Valor de Habilidade", Descricao = "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica." },
            new CaracteristicaClasse { Id = 6, IdClasse = 1, Nivel = 6, Nome = "Característica de Patrono Transcendental", Descricao = "Seu Patrono Transcendental concede a você uma característica no 6º nível baseado na sua escolha de Patrono." },
            new CaracteristicaClasse { Id = 7, IdClasse = 1, Nivel = 8, Nome = "Incremento no Valor de Habilidade", Descricao = "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica." },
            new CaracteristicaClasse { Id = 8, IdClasse = 1, Nivel = 10, Nome = "Característica de Patrono Transcendental", Descricao = "Seu Patrono Transcendental concede a você uma característica no 10º nível baseado na sua escolha de Patrono." },
            new CaracteristicaClasse { Id = 9, IdClasse = 1, Nivel = 11, Nome = "Arcana Mística (6° nível)", Descricao = "Seu patrono confere a você um segredo mágico chamado de Arcana Mística. Escolha uma magia de 6º nível da lista de magias de bruxo como sua arcana mística. Você pode conjurar essa magia uma vez sem gastar um espaço de magia. Você deve terminar um descanso longo antes de poder fazer isso novamente." },
            new CaracteristicaClasse { Id = 10, IdClasse = 1, Nivel = 12, Nome = "Incremento no Valor de Habilidade", Descricao = "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica." },
            new CaracteristicaClasse { Id = 11, IdClasse = 1, Nivel = 13, Nome = "Arcana Mística (7° nível)", Descricao = "Seu patrono confere a você um segredo mágico chamado de Arcana Mística. Escolha uma magia de 7º nível da lista de magias de bruxo como sua arcana mística. Você pode conjurar essa magia uma vez sem gastar um espaço de magia. Você deve terminar um descanso longo antes de poder fazer isso novamente." },
            new CaracteristicaClasse { Id = 12, IdClasse = 1, Nivel = 14, Nome = "Característica de Patrono Transcendental", Descricao = "Seu Patrono Transcendental concede a você uma característica no 14º nível baseado na sua escolha de Patrono." },
            new CaracteristicaClasse { Id = 13, IdClasse = 1, Nivel = 15, Nome = "Arcana Mística (8° nível)", Descricao = "Seu patrono confere a você um segredo mágico chamado de Arcana Mística. Escolha uma magia de 8º nível da lista de magias de bruxo como sua arcana mística. Você pode conjurar essa magia uma vez sem gastar um espaço de magia. Você deve terminar um descanso longo antes de poder fazer isso novamente." },
            new CaracteristicaClasse { Id = 14, IdClasse = 1, Nivel = 16, Nome = "Incremento no Valor de Habilidade", Descricao = "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica." },
            new CaracteristicaClasse { Id = 15, IdClasse = 1, Nivel = 17, Nome = "Arcana Mística (9° nível)", Descricao = "Seu patrono confere a você um segredo mágico chamado de Arcana Mística. Escolha uma magia de 9º nível da lista de magias de bruxo como sua arcana mística. Você pode conjurar essa magia uma vez sem gastar um espaço de magia. Você deve terminar um descanso longo antes de poder fazer isso novamente." },
            new CaracteristicaClasse { Id = 16, IdClasse = 1, Nivel = 19, Nome = "Incremento no Valor de Habilidade", Descricao = "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica." },
            new CaracteristicaClasse { Id = 17, IdClasse = 1, Nivel = 20, Nome = "Mestre Místico", Descricao = "No 20º nível, você pode extrair de sua reserva interna de poder místico enquanto roga ao seu patrono para recuperar espaços de magia gastos. Você pode gastar 1 minuto implorando pela ajuda do seu patrono para recuperar todos os seus espaços de magia gastos da sua característica Magia de Pacto. Você deve terminar um descanso longo antes de usar esta característica novamente." }
        );

        // Seeding Magias
        modelBuilder.Entity<Magia>().HasData(
            new Magia
            {
                Id = 1,
                Nome = "Raio místico",
                Nivel = 0,
                Descricao = "Um feixe de energia estalante vai em direção a uma criatura. Faça um ataque à distância com magia. Com um acerto, o alvo sofre 1d10 de dano de energia. **Explosão Agonizante:** Você adiciona seu Modificador de Carisma (+4) ao dano. Dano total por acerto: 1d10+4 de Energia."
            }
        );

        // Seeding PersonagensMagias
        modelBuilder.Entity<PersonagemMagia>().HasData(
            new PersonagemMagia { IdPersonagem = 1, IdMagia = 1 }
        );

        // Seeding Pericias
        modelBuilder.Entity<Pericia>().HasData(
            new Pericia { Id = 1, Nome = "Acrobacia", ModificadorAtributo = "Destreza" },
            new Pericia { Id = 2, Nome = "Adestrar Animais", ModificadorAtributo = "Sabedoria" },
            new Pericia { Id = 3, Nome = "Arcana", ModificadorAtributo = "Inteligência" },
            new Pericia { Id = 4, Nome = "Atletismo", ModificadorAtributo = "Força" },
            new Pericia { Id = 5, Nome = "Atuação", ModificadorAtributo = "Carisma" },
            new Pericia { Id = 6, Nome = "Enganação", ModificadorAtributo = "Carisma" },
            new Pericia { Id = 7, Nome = "Furtividade", ModificadorAtributo = "Destreza" },
            new Pericia { Id = 8, Nome = "História", ModificadorAtributo = "Inteligência" },
            new Pericia { Id = 9, Nome = "Intimidação", ModificadorAtributo = "Carisma" },
            new Pericia { Id = 10, Nome = "Intuição", ModificadorAtributo = "Sabedoria" },
            new Pericia { Id = 11, Nome = "Investigação", ModificadorAtributo = "Inteligência" },
            new Pericia { Id = 12, Nome = "Medicina", ModificadorAtributo = "Sabedoria" },
            new Pericia { Id = 13, Nome = "Natureza", ModificadorAtributo = "Inteligência" },
            new Pericia { Id = 14, Nome = "Percepção", ModificadorAtributo = "Sabedoria" },
            new Pericia { Id = 15, Nome = "Persuasão", ModificadorAtributo = "Carisma" },
            new Pericia { Id = 16, Nome = "Prestidigitação", ModificadorAtributo = "Destreza" },
            new Pericia { Id = 17, Nome = "Religião", ModificadorAtributo = "Inteligência" },
            new Pericia { Id = 18, Nome = "Sobrevivência", ModificadorAtributo = "Sabedoria" }
        );

        // Seeding Kairo's Chapters (IdPersonagem = 1)
        modelBuilder.Entity<HistoriaPersonagem>().HasData(
            new HistoriaPersonagem
            {
                Id = 1,
                IdPersonagem = 1,
                Capitulo = 1,
                TituloCapitulo = "O Pecado do Eclipse",
                Texto = """
Em Feywild, as leis não são escritas em papel, são moldadas em magia e tradição. A Corte de Verão, guiada pela paixão inflamável, e a Corte de Inverno, regida pela frieza calculista, mantinham um distanciamento hostil há milênios. Mas a mãe de Nailo, uma nobre cavaleira do Verão, e seu pai, um estrategista do Inverno, cometeram o maior dos crimes: eles se apaixonaram.

A união deles foi um segredo mantido a duras penas, até o dia em que Nailo nasceu. O choro da criança ecoou pela floresta mágica, e sua pele irradiava uma energia que oscilava entre o calor reconfortante e a brisa gélida. Ele era a prova viva de que a separação das cortes era uma mentira política. Quando os espiões feéricos descobriram o bebê, a notícia correu como fogo seco. A criança foi apelidada de "O Eclipse" — uma anomalia que ameaçava o status quo. Ambas as rainhas exigiram que a "mancha" fosse apagada. Sem exércitos ou aliados, os pais de Nailo abandonaram seus postos, seus títulos e fugiram para os ermos sombrios da Agrestia das Fadas.
"""
            },
            new HistoriaPersonagem
            {
                Id = 2,
                IdPersonagem = 1,
                Capitulo = 2,
                TituloCapitulo = "A Infância nas Sombras",
                Texto = """
Durante dez anos, a vida de Nailo foi uma eterna mudança. Ele não conhecia grandes castelos de cristal ou os bailes eternos do Verão. Seu mundo eram acampamentos escondidos, cavernas camufladas e o amor incondicional de pais que viviam com os ouvidos atentos a qualquer galho quebrado.

Apesar da vida dura e do medo constante, Nailo encontrou seu refúgio na Primavera. Seus pais faziam de tudo para blindá-lo do terror da perseguição, ensinando-o a ver a beleza nas pequenas coisas e a tirar alegria de qualquer situação. Ele aprendeu a se mover em silêncio absoluto observando a postura defensiva do pai e herdou o sorriso invencível da mãe. Em seus momentos de distração, quando se sentia seguro o suficiente para rir solto, seus olhos e a magia ao seu redor brilhavam em um tom de amarelo extremamente vivo e chamativo que contrastava com a escuridão da floresta em que se escondiam.
"""
            },
            new HistoriaPersonagem
            {
                Id = 3,
                IdPersonagem = 1,
                Capitulo = 3,
                TituloCapitulo = "A Fenda Antiga",
                Texto = """
O fim da paz provisória chegou na noite do décimo aniversário de Nailo. Os cães de caça da Corte de Inverno e os batedores implacáveis do Verão os encurralaram simultaneamente em um desfiladeiro. Sem saída, seus pais sacaram as armas, trocando um último olhar de cumplicidade. A ordem para o filho foi clara: Corra e não olhe para trás.

Nailo correu desesperado, com o som do combate e dos feitiços explodindo às suas costas. Em seu pânico, o chão cedeu. Ele rolou por uma encosta de terra escura e caiu dentro de uma fenda profunda, aterrissando no chão empoeirado de ruínas antigas e esquecidas até mesmo pelas fadas. O som das garras dos cães se aproximava rapidamente do buraco acima dele.

Tateando no escuro, sua mão esbarrou em algo: um pergaminho antigo (scroll) coberto por hieróglifos incompreensíveis. Preso a ele, havia um pequeno pedaço de papel desgastado com três palavras soltas em élfico antigo. Com as criaturas rosnando e começando a descer pela fenda, Nailo, tremendo, leu as palavras em voz alta. Ao acertar a pronúncia da última sílaba mágica, o pergaminho se despedaçou em chispas de luz, e o mundo ao redor do menino foi engolido pelo vazio.
"""
            },
            new HistoriaPersonagem
            {
                Id = 4,
                IdPersonagem = 1,
                Capitulo = 4,
                TituloCapitulo = "A Promessa de Dedinho",
                Texto = """
O silêncio substituiu os rosnados. Nailo abriu os olhos e se viu em uma pequena sala de pedra fria. O chão estava coberto de símbolos arcanos desenhados a giz, papéis antigos voavam levemente e uma corrente colossal prendia a figura central da sala. No meio de um círculo ritualístico, encontrava-se um Djinni azul gigantesco. Mesmo sentado, a criatura era monumental — se Nailo ficasse de pé ao seu lado, mal bateria na altura do peito do ser elemental. Mais ao canto, jogada no chão como lixo, repousava uma adaga.

A criança da Primavera, assustada com o que havia acontecido a segundos atrás, e confusa com o que estava acontecendo agora, se apresentou. Limpando a poeira das calças, o Djinni, surpreso, quebrou o silêncio.
— A companhia não me desagrada — retumbou a voz profunda da entidade.

Durante a conversa, o Djinni tentou explicar o nó mágico de seu tormento. Um feiticeiro antigo o havia selado dentro do semiplano contido naquela adaga, mas cometeu o requinte de crueldade de jogar a própria adaga para dentro da prisão. Um paradoxo mágico perfeito. Era impossível que a magia fosse quebrada de dentro para fora com os recursos que tinham ali, e na situação que eles se encontravam.

— Mas você não faz parte dessa equação, criança — explicou o Gênio, apontando para uma porta negra no fundo da sala. — Você não está selado. Pode simplesmente passar por aquela porta e ir embora.

Nailo olhou para a porta, depois para as correntes pesadas, e caminhou até o gigante azul. Ignorando as regras complexas da magia arcana, he esticou a mãozinha e levantou o dedo mindinho em direção ao peito do Djinni.

— Eu prometo de dedinho que mesmo que eu saia, eu irei voltar para te tirar daqui. Ninguém merece ficar aprisionado assim sem motivo.

O Gênio arregalou os olhos. Em Feywild, uma promessa infantil carregava um peso cósmico imensurável. O conceito de confiança e amizade havia se perdido há séculos para o elemental, mas ali, diante de uma criança sorridente, o pacto arcano mais improvável da história foi forjado através de um mindinho estendido.

O Djinni riu, uma risada rouca e genuína que fez a sala tremer, e aceitou a promessa, infundindo a primeira faísca de poder arcano na alma do garoto.

Nailo pegou a adaga do chão, caminhou até a porta negra e abriu a maçaneta. Antes de atravessar o umbral que o levaria para longe do plano da adaga, he olhou por cima do ombro, sorriu largo e disse:

— Até mais.
"""
            },
            new HistoriaPersonagem
            {
                Id = 5,
                IdPersonagem = 1,
                Capitulo = 5,
                TituloCapitulo = "O \"Azulão\" e a Floresta",
                Texto = """
Assim que a porta negra bateu às suas costas, Kairo se viu no meio de uma planície vasta, espremida entre as encostas de uma grande montanha e a floresta densa e mística de Mythos. Foi só quando o silêncio da noite o abraçou que o garoto arregalou os olhos: em meio a toda a confusão e promessas, ele havia esquecido de perguntar o nome da entidade milenar que acabou de salvar sua vida. A partir daquele dia, o ser todo-poderoso seria chamado apenas de "Gênio", "Amigão" ou "Azulão".

Os primeiros anos na floresta foram de sobrevivência crua. Sem a proteção dos pais, Kairo dependia dos estrondos mágicos de seu Raio Místico — canalizados pela adaga — para abater animais. O trauma da perda cobrou seu preço. Durante muito tempo, as noites de transe feérico de Kairo eram conturbadas, e he acordava flutuando violentamente entre as estações: dias de Inverno profundo, onde a tristeza gélida não o deixava sair da caverna, seguidos de dias de Verão, onde a fúria flamejante o fazia destruir árvores com magia.

Porém, a resiliência falou mais alto. Kairo percebeu que ceder à dor seria esquecer o que seus pais haviam se sacrificado para proteger. Após meses meditando, ele conseguiu ancorar sua alma. Decidiu que enfrentaria o mundo com o mesmo sorriso invencível de sua mãe, mas também com o olhar calculista de seu pai, estabilizando-se no centro em sua forma de Primavera.
"""
            },
            new HistoriaPersonagem
            {
                Id = 6,
                IdPersonagem = 1,
                Capitulo = 6,
                TituloCapitulo = "O Valor do Ouro e a Máscara de Inverno",
                Texto = """
A floresta, embora familiar, tornou-se solitária demais para um Eladrin da Primavera. As luzes de Ozenes, uma cidade portuária ao longe, o atraíram. Antes de se aventurar pelas ruas, Kairo passou dias escondido nos telhados, observando a dinâmica humana. Ele notou que as pessoas entregavam pequenos discos dourados umas às outras em troca de peixe e pão. Confuso, he entrou no semiplano da adaga e perguntou ao Azulão. O Gênio, com um suspiro cansado, explicou o conceito civilizado de "dinheiro".

Com a lição anotada, Kairo traçou um plano. Ele entrou em transe, forçando sua mente a assumir a frieza do Inverno. Na manhã seguinte, com cabelos lisos, brancos e pele pálida, o garoto sorrateiro roubou duas moedas de ouro de mercadores desatentos, escondendo-se antes do pôr do sol. No outro dia, ele acordou em sua radiante forma de Primavera: cabelos verdes esvoaçantes e pele brilhante. Kairo entrou na taverna local e, achando que duas moedas de ouro eram o equivalente a uma fortuna inesgotável, pediu tudo o que tinha direito. Ele devorou porções infinitas de peixe fatiado e bolinhos de arroz.

Quando a conta chegou, o choque: as moedas não cobriam o banquete. Para não ser jogado no mar, o jovem precisou lavar pratos e limpar mesas por semanas. O castigo, no entanto, foi sua maior escola. Trabalhando ali, Kairo aprendeu a ler as pessoas, a ouvir boatos e a entender como Ozenes funcionava. Logo, ele passou a fazer pequenos bicos carregando cargas para os mercadores no cais, intercalando com dias ocasionais de "Inverno" para realizar furtos precisos quando o dinheiro faltava. Um ladrão de cabelos brancos roubava à noite, e um trabalhador sorridente de cabelos verdes gastava o dinheiro de dia.
"""
            },
            new HistoriaPersonagem
            {
                Id = 7,
                IdPersonagem = 1,
                Capitulo = 7,
                TituloCapitulo = "Goblins, Solidão e o Tomo Arcano",
                Texto = """
Aos poucos, carregar caixotes perdeu a graça, e Kairo se registrou na Guilda local para missões de escolta e proteção nas estradas. Foi assim que ele conheceu seu maior obstáculo: os Goblins. Passando semanas emboscado nas matas ao redor da cidade e observando os acampamentos dessas criaturas, a inteligência de Kairo brilhou. Sem professor algum, apenas ouvindo e deduzindo, ele aprendeu o idioma Goblin na marra, o que o tornou um aventureiro muito mais eficiente.

O Gênio assistia a tudo isso de dentro da lâmina. A entidade milenar, que a princípio via Kairo apenas como um bilhete de saída aleatório, começou a perceber o verdadeiro potencial do garoto. Ele levava jeito para a coisa. Contudo, o Azulão também notou algo que o sorriso da Primavera tentava esconder: nos momentos de silêncio, a solidão de Kairo era ensurdecedora.

Em uma de suas visitas à adaga, o Gênio materializou um livro de couro escuro e o jogou no colo de Kairo.
— Você tem sido útil, criança. Mas está canalizando magia como um amador — resmungou o Gênio, disfarçando qualquer traço de empatia com seu tom pragmático de sempre. — Use isso. Tem algumas anotações que podem evitar que você morra de forma estúpida e me deixe preso aqui para sempre.

Aquele era o seu Livro das Sombras. Ao folhear as páginas rúnicas, Kairo encontrou feitiços novos, mas um em especial chamou sua atenção: Encontrar Familiar. Naquela mesma noite, em seu quarto alugado em Ozenes, Kairo conjurou a magia. Das sombras e da luz feérica, surgiu Shift. O pequeno companheiro animal — que mudava de forma dependendo do dia, ora um lagarto sorrateiro, ora um falcão ágil — tornou-se seu companheiro.

Agora, aos 20 anos, armado com seu sorriso extravagante, seu fiel familiar Shift, a magia trovejante de um Gênio que virou um amigo rabugento e uma promessa de dedinho que ecoava pelo multiverso, Kairo estava pronto para deixar Ozenes e abraçar seu destiny.
"""
            }
        );

        // Seeding TracoRacialPericia
        modelBuilder.Entity<TracoRacialPericia>().HasData(
            // Sentidos Aguçados (Id = 4) dá proficiência em Percepção (Id = 14)
            new TracoRacialPericia { IdTracoRacial = 4, IdPericia = 14, IsMaestria = false }
        );

        // Seeding ClassePericiaDisponivel (Bruxo pode escolher entre as seguintes)
        modelBuilder.Entity<ClassePericiaDisponivel>().HasData(
            new ClassePericiaDisponivel { IdClasse = 1, IdPericia = 3 },  // Arcana
            new ClassePericiaDisponivel { IdClasse = 1, IdPericia = 6 },  // Enganação
            new ClassePericiaDisponivel { IdClasse = 1, IdPericia = 8 },  // História
            new ClassePericiaDisponivel { IdClasse = 1, IdPericia = 9 },  // Intimidação
            new ClassePericiaDisponivel { IdClasse = 1, IdPericia = 11 }, // Investigação
            new ClassePericiaDisponivel { IdClasse = 1, IdPericia = 13 }, // Natureza
            new ClassePericiaDisponivel { IdClasse = 1, IdPericia = 17 }  // Religião
        );

        // Seeding PersonagemPericia para Kairo (IdPersonagem = 1)
        modelBuilder.Entity<PersonagemPericia>().HasData(
            new PersonagemPericia { IdPersonagem = 1, IdPericia = 3, IsProficiente = true, IsMaestria = false, Origem = "Classe (Bruxo)" },
            new PersonagemPericia { IdPersonagem = 1, IdPericia = 6, IsProficiente = true, IsMaestria = false, Origem = "Classe (Bruxo)" },
            new PersonagemPericia { IdPersonagem = 1, IdPericia = 8, IsProficiente = true, IsMaestria = false, Origem = "Antecedente (Charlatão)" },
            new PersonagemPericia { IdPersonagem = 1, IdPericia = 7, IsProficiente = true, IsMaestria = false, Origem = "Antecedente (Charlatão)" }
        );

        // Seeding PersonagemSalvaguarda para Kairo
        modelBuilder.Entity<PersonagemSalvaguarda>().HasData(
            new PersonagemSalvaguarda { IdPersonagem = 1, Atributo = "Sabedoria", IsProficiente = true },
            new PersonagemSalvaguarda { IdPersonagem = 1, Atributo = "Carisma", IsProficiente = true }
        );

        // Seeding PersonagemProficiencia para Kairo
        modelBuilder.Entity<PersonagemProficiencia>().HasData(
            new PersonagemProficiencia { Id = 1, IdPersonagem = 1, Tipo = "Arma", Nome = "Armas simples", Origem = "Classe (Bruxo)" },
            new PersonagemProficiencia { Id = 2, IdPersonagem = 1, Tipo = "Armadura", Nome = "Armaduras leves", Origem = "Classe (Bruxo)" },
            new PersonagemProficiencia { Id = 3, IdPersonagem = 1, Tipo = "Ferramenta", Nome = "Nenhuma", Origem = "Classe (Bruxo)" }
        );

        // Seeding AcaoPersonagem para Kairo
        modelBuilder.Entity<AcaoPersonagem>().HasData(
            new AcaoPersonagem
            {
                Id = 1,
                IdPersonagem = 1,
                Nome = "Soco",
                TipoAcao = "Ação",
                Alcance = "1.5m / 5ft",
                BonusAcerto = "+1",
                Dano = "1",
                TipoDano = "Impacto",
                Descricao = "Ataque desarmado básico.",
                AcertoTooltip = "[FOR](FOR) + [PROF](Proficiência)",
                DanoTooltip = "1 (Base) + [FOR](FOR)"
            }
        );

        // Seeding Idiomas
        modelBuilder.Entity<Idioma>().HasData(
            new Idioma { Id = 1, Nome = "Comum" },
            new Idioma { Id = 2, Nome = "Anão" },
            new Idioma { Id = 3, Nome = "Élfico" },
            new Idioma { Id = 4, Nome = "Gigante" },
            new Idioma { Id = 5, Nome = "Gnomo" },
            new Idioma { Id = 6, Nome = "Goblin" },
            new Idioma { Id = 7, Nome = "Halfling" },
            new Idioma { Id = 8, Nome = "Orc" }
        );

        // Seeding PersonagemIdioma para Kairo
        modelBuilder.Entity<PersonagemIdioma>().HasData(
            new PersonagemIdioma { IdPersonagem = 1, IdIdioma = 1 }
        );
    }
}
