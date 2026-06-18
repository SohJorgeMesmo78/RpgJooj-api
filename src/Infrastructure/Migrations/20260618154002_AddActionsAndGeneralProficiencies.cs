using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActionsAndGeneralProficiencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Origem",
                table: "PersonagensPericias",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QtdPericiasEscolha",
                table: "Classes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AcoesPersonagens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersonagem = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TipoAcao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Alcance = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BonusAcerto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Dano = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TipoDano = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcoesPersonagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcoesPersonagens_Personagens_IdPersonagem",
                        column: x => x.IdPersonagem,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassesPericiasDisponiveis",
                columns: table => new
                {
                    IdClasse = table.Column<int>(type: "integer", nullable: false),
                    IdPericia = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassesPericiasDisponiveis", x => new { x.IdClasse, x.IdPericia });
                    table.ForeignKey(
                        name: "FK_ClassesPericiasDisponiveis_Classes_IdClasse",
                        column: x => x.IdClasse,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassesPericiasDisponiveis_Pericias_IdPericia",
                        column: x => x.IdPericia,
                        principalTable: "Pericias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonagensProficiencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersonagem = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Origem = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonagensProficiencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonagensProficiencias_Personagens_IdPersonagem",
                        column: x => x.IdPersonagem,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonagensSalvaguardas",
                columns: table => new
                {
                    IdPersonagem = table.Column<int>(type: "integer", nullable: false),
                    Atributo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsProficiente = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonagensSalvaguardas", x => new { x.IdPersonagem, x.Atributo });
                    table.ForeignKey(
                        name: "FK_PersonagensSalvaguardas_Personagens_IdPersonagem",
                        column: x => x.IdPersonagem,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AcoesPersonagens",
                columns: new[] { "Id", "Alcance", "BonusAcerto", "Dano", "Descricao", "IdPersonagem", "Nome", "TipoAcao", "TipoDano" },
                values: new object[] { 1, "1.5m / 5ft", "+1", "1", "Ataque desarmado básico.", 1, "Soco", "Ação", "Impacto" });

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1,
                column: "QtdPericiasEscolha",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2,
                column: "QtdPericiasEscolha",
                value: 2);

            migrationBuilder.InsertData(
                table: "ClassesPericiasDisponiveis",
                columns: new[] { "IdClasse", "IdPericia" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 1, 6 },
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 11 },
                    { 1, 13 },
                    { 1, 17 }
                });

            migrationBuilder.UpdateData(
                table: "PersonagensPericias",
                keyColumns: new[] { "IdPericia", "IdPersonagem" },
                keyValues: new object[] { 3, 1 },
                column: "Origem",
                value: "Classe (Bruxo)");

            migrationBuilder.UpdateData(
                table: "PersonagensPericias",
                keyColumns: new[] { "IdPericia", "IdPersonagem" },
                keyValues: new object[] { 6, 1 },
                column: "Origem",
                value: "Classe (Bruxo)");

            migrationBuilder.UpdateData(
                table: "PersonagensPericias",
                keyColumns: new[] { "IdPericia", "IdPersonagem" },
                keyValues: new object[] { 7, 1 },
                column: "Origem",
                value: "Antecedente (Charlatão)");

            migrationBuilder.UpdateData(
                table: "PersonagensPericias",
                keyColumns: new[] { "IdPericia", "IdPersonagem" },
                keyValues: new object[] { 8, 1 },
                column: "Origem",
                value: "Antecedente (Charlatão)");

            migrationBuilder.InsertData(
                table: "PersonagensProficiencias",
                columns: new[] { "Id", "IdPersonagem", "Nome", "Origem", "Tipo" },
                values: new object[,]
                {
                    { 1, 1, "Armas simples", "Classe (Bruxo)", "Arma" },
                    { 2, 1, "Armaduras leves", "Classe (Bruxo)", "Armadura" },
                    { 3, 1, "Nenhuma", "Classe (Bruxo)", "Ferramenta" }
                });

            migrationBuilder.InsertData(
                table: "PersonagensSalvaguardas",
                columns: new[] { "Atributo", "IdPersonagem", "IsProficiente" },
                values: new object[,]
                {
                    { "Carisma", 1, true },
                    { "Sabedoria", 1, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcoesPersonagens_IdPersonagem",
                table: "AcoesPersonagens",
                column: "IdPersonagem");

            migrationBuilder.CreateIndex(
                name: "IX_ClassesPericiasDisponiveis_IdPericia",
                table: "ClassesPericiasDisponiveis",
                column: "IdPericia");

            migrationBuilder.CreateIndex(
                name: "IX_PersonagensProficiencias_IdPersonagem",
                table: "PersonagensProficiencias",
                column: "IdPersonagem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcoesPersonagens");

            migrationBuilder.DropTable(
                name: "ClassesPericiasDisponiveis");

            migrationBuilder.DropTable(
                name: "PersonagensProficiencias");

            migrationBuilder.DropTable(
                name: "PersonagensSalvaguardas");

            migrationBuilder.DropColumn(
                name: "Origem",
                table: "PersonagensPericias");

            migrationBuilder.DropColumn(
                name: "QtdPericiasEscolha",
                table: "Classes");
        }
    }
}
