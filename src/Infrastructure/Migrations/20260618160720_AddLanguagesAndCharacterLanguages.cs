using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguagesAndCharacterLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Idiomas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idiomas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonagensIdiomas",
                columns: table => new
                {
                    IdPersonagem = table.Column<int>(type: "integer", nullable: false),
                    IdIdioma = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonagensIdiomas", x => new { x.IdPersonagem, x.IdIdioma });
                    table.ForeignKey(
                        name: "FK_PersonagensIdiomas_Idiomas_IdIdioma",
                        column: x => x.IdIdioma,
                        principalTable: "Idiomas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonagensIdiomas_Personagens_IdPersonagem",
                        column: x => x.IdPersonagem,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Idiomas",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Comum" },
                    { 2, "Anão" },
                    { 3, "Élfico" },
                    { 4, "Gigante" },
                    { 5, "Gnomo" },
                    { 6, "Goblin" },
                    { 7, "Halfling" },
                    { 8, "Orc" }
                });

            migrationBuilder.InsertData(
                table: "PersonagensIdiomas",
                columns: new[] { "IdIdioma", "IdPersonagem" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_PersonagensIdiomas_IdIdioma",
                table: "PersonagensIdiomas",
                column: "IdIdioma");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonagensIdiomas");

            migrationBuilder.DropTable(
                name: "Idiomas");
        }
    }
}
