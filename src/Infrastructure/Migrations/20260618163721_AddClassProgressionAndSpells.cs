using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClassProgressionAndSpells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subclasse",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "IdSubclasse",
                table: "ClassesPersonagens",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdClassePai",
                table: "Classes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClasseProgressoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdClasse = table.Column<int>(type: "integer", nullable: false),
                    Nivel = table.Column<int>(type: "integer", nullable: false),
                    BonusProficiencia = table.Column<int>(type: "integer", nullable: false),
                    TruquesConhecidos = table.Column<int>(type: "integer", nullable: false),
                    MagiasConhecidas = table.Column<int>(type: "integer", nullable: false),
                    EspacosMagia = table.Column<int>(type: "integer", nullable: false),
                    NivelMagia = table.Column<int>(type: "integer", nullable: false),
                    InvocacoesConhecidas = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClasseProgressoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClasseProgressoes_Classes_IdClasse",
                        column: x => x.IdClasse,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Magias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nivel = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonagensMagias",
                columns: table => new
                {
                    IdPersonagem = table.Column<int>(type: "integer", nullable: false),
                    IdMagia = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonagensMagias", x => new { x.IdPersonagem, x.IdMagia });
                    table.ForeignKey(
                        name: "FK_PersonagensMagias_Magias_IdMagia",
                        column: x => x.IdMagia,
                        principalTable: "Magias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonagensMagias_Personagens_IdPersonagem",
                        column: x => x.IdPersonagem,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClasseProgressoes",
                columns: new[] { "Id", "BonusProficiencia", "EspacosMagia", "IdClasse", "InvocacoesConhecidas", "MagiasConhecidas", "Nivel", "NivelMagia", "TruquesConhecidos" },
                values: new object[] { 1, 2, 1, 1, 0, 2, 1, 1, 2 });

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1,
                column: "IdClassePai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2,
                column: "IdClassePai",
                value: null);

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "DadoVida", "Deslocamento", "IdClassePai", "Nome", "QtdPericiasEscolha" },
                values: new object[,]
                {
                    { 3, "", null, 1, "o Gênio", 0 },
                    { 4, "", null, 2, "Caminho do eu astral", 0 }
                });

            migrationBuilder.UpdateData(
                table: "ClassesPersonagens",
                keyColumns: new[] { "IdClasse", "IdPersonagem" },
                keyValues: new object[] { 1, 1 },
                column: "IdSubclasse",
                value: 3);

            migrationBuilder.InsertData(
                table: "Magias",
                columns: new[] { "Id", "Descricao", "Nivel", "Nome" },
                values: new object[] { 1, "Um feixe de energia estalante vai em direção a uma criatura. Faça um ataque à distância com magia. Com um acerto, o alvo sofre 1d10 de dano de energia. **Explosão Agonizante:** Você adiciona seu Modificador de Carisma (+4) ao dano. Dano total por acerto: 1d10+4 de Energia.", 0, "Raio místico" });

            migrationBuilder.InsertData(
                table: "PersonagensMagias",
                columns: new[] { "IdMagia", "IdPersonagem" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ClassesPersonagens_IdSubclasse",
                table: "ClassesPersonagens",
                column: "IdSubclasse");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_IdClassePai",
                table: "Classes",
                column: "IdClassePai");

            migrationBuilder.CreateIndex(
                name: "IX_ClasseProgressoes_IdClasse",
                table: "ClasseProgressoes",
                column: "IdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_PersonagensMagias_IdMagia",
                table: "PersonagensMagias",
                column: "IdMagia");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Classes_IdClassePai",
                table: "Classes",
                column: "IdClassePai",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassesPersonagens_Classes_IdSubclasse",
                table: "ClassesPersonagens",
                column: "IdSubclasse",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Classes_IdClassePai",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassesPersonagens_Classes_IdSubclasse",
                table: "ClassesPersonagens");

            migrationBuilder.DropTable(
                name: "ClasseProgressoes");

            migrationBuilder.DropTable(
                name: "PersonagensMagias");

            migrationBuilder.DropTable(
                name: "Magias");

            migrationBuilder.DropIndex(
                name: "IX_ClassesPersonagens_IdSubclasse",
                table: "ClassesPersonagens");

            migrationBuilder.DropIndex(
                name: "IX_Classes_IdClassePai",
                table: "Classes");

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "IdSubclasse",
                table: "ClassesPersonagens");

            migrationBuilder.DropColumn(
                name: "IdClassePai",
                table: "Classes");

            migrationBuilder.AddColumn<string>(
                name: "Subclasse",
                table: "Classes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Subclasse",
                value: "o Gênio");

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Subclasse",
                value: "Caminho do eu astral");
        }
    }
}
