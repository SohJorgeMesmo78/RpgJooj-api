using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDnDSheetSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classe",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Subclasse",
                table: "Personagens");

            migrationBuilder.RenameColumn(
                name: "Nivel",
                table: "Personagens",
                newName: "Sabedoria");

            migrationBuilder.AddColumn<int>(
                name: "Carisma",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Constituicao",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Destreza",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Forca",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Inteligencia",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Subclasse = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DadoVida = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Deslocamento = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pericias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModificadorAtributo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pericias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassesPersonagens",
                columns: table => new
                {
                    IdPersonagem = table.Column<int>(type: "integer", nullable: false),
                    IdClasse = table.Column<int>(type: "integer", nullable: false),
                    Nivel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassesPersonagens", x => new { x.IdPersonagem, x.IdClasse });
                    table.ForeignKey(
                        name: "FK_ClassesPersonagens_Classes_IdClasse",
                        column: x => x.IdClasse,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassesPersonagens_Personagens_IdPersonagem",
                        column: x => x.IdPersonagem,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "DadoVida", "Deslocamento", "Nome", "Subclasse" },
                values: new object[,]
                {
                    { 1, "d8", 9, "Bruxo", "o Gênio" },
                    { 2, "d8", 12, "Monge", "Caminho do eu astral" }
                });

            migrationBuilder.InsertData(
                table: "Pericias",
                columns: new[] { "Id", "ModificadorAtributo", "Nome" },
                values: new object[,]
                {
                    { 1, "Destreza", "Acrobacia" },
                    { 2, "Sabedoria", "Adestrar Animais" },
                    { 3, "Inteligência", "Arcana" },
                    { 4, "Força", "Atletismo" },
                    { 5, "Carisma", "Atuação" },
                    { 6, "Carisma", "Enganação" },
                    { 7, "Destreza", "Furtividade" },
                    { 8, "Inteligência", "História" },
                    { 9, "Carisma", "Intimidação" },
                    { 10, "Sabedoria", "Intuição" },
                    { 11, "Inteligência", "Investigação" },
                    { 12, "Sabedoria", "Medicina" },
                    { 13, "Inteligência", "Natureza" },
                    { 14, "Sabedoria", "Percepção" },
                    { 15, "Carisma", "Persuasão" },
                    { 16, "Destreza", "Prestidigitação" },
                    { 17, "Inteligência", "Religião" },
                    { 18, "Sabedoria", "Sobrevivência" }
                });

            migrationBuilder.UpdateData(
                table: "Personagens",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Carisma", "Constituicao", "Destreza", "Forca", "Inteligencia", "Sabedoria" },
                values: new object[] { 19, 13, 16, 8, 13, 12 });

            migrationBuilder.UpdateData(
                table: "Personagens",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Carisma", "Constituicao", "Destreza", "Forca", "Inteligencia", "Sabedoria" },
                values: new object[] { 8, 14, 16, 12, 10, 16 });

            migrationBuilder.InsertData(
                table: "ClassesPersonagens",
                columns: new[] { "IdClasse", "IdPersonagem", "Nivel" },
                values: new object[,]
                {
                    { 1, 1, 3 },
                    { 2, 2, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassesPersonagens_IdClasse",
                table: "ClassesPersonagens",
                column: "IdClasse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassesPersonagens");

            migrationBuilder.DropTable(
                name: "Pericias");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropColumn(
                name: "Carisma",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Constituicao",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Destreza",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Forca",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Inteligencia",
                table: "Personagens");

            migrationBuilder.RenameColumn(
                name: "Sabedoria",
                table: "Personagens",
                newName: "Nivel");

            migrationBuilder.AddColumn<string>(
                name: "Classe",
                table: "Personagens",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subclasse",
                table: "Personagens",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Personagens",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Classe", "Nivel", "Subclasse" },
                values: new object[] { "Bruxo", 3, "o Gênio" });

            migrationBuilder.UpdateData(
                table: "Personagens",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Classe", "Nivel", "Subclasse" },
                values: new object[] { "Monge", 5, "Caminho do eu astral" });
        }
    }
}
