using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRacialAndManualSkillsRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonagensPericias",
                columns: table => new
                {
                    IdPersonagem = table.Column<int>(type: "integer", nullable: false),
                    IdPericia = table.Column<int>(type: "integer", nullable: false),
                    IsProficiente = table.Column<bool>(type: "boolean", nullable: false),
                    IsMaestria = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonagensPericias", x => new { x.IdPersonagem, x.IdPericia });
                    table.ForeignKey(
                        name: "FK_PersonagensPericias_Pericias_IdPericia",
                        column: x => x.IdPericia,
                        principalTable: "Pericias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonagensPericias_Personagens_IdPersonagem",
                        column: x => x.IdPersonagem,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TracosRaciaisPericias",
                columns: table => new
                {
                    IdTracoRacial = table.Column<int>(type: "integer", nullable: false),
                    IdPericia = table.Column<int>(type: "integer", nullable: false),
                    IsMaestria = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TracosRaciaisPericias", x => new { x.IdTracoRacial, x.IdPericia });
                    table.ForeignKey(
                        name: "FK_TracosRaciaisPericias_Pericias_IdPericia",
                        column: x => x.IdPericia,
                        principalTable: "Pericias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TracosRaciaisPericias_TracosRaciais_IdTracoRacial",
                        column: x => x.IdTracoRacial,
                        principalTable: "TracosRaciais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PersonagensPericias",
                columns: new[] { "IdPericia", "IdPersonagem", "IsMaestria", "IsProficiente" },
                values: new object[,]
                {
                    { 3, 1, false, true },
                    { 6, 1, false, true },
                    { 7, 1, false, true },
                    { 8, 1, false, true }
                });

            migrationBuilder.InsertData(
                table: "TracosRaciaisPericias",
                columns: new[] { "IdPericia", "IdTracoRacial", "IsMaestria" },
                values: new object[] { 14, 4, false });

            migrationBuilder.CreateIndex(
                name: "IX_PersonagensPericias_IdPericia",
                table: "PersonagensPericias",
                column: "IdPericia");

            migrationBuilder.CreateIndex(
                name: "IX_TracosRaciaisPericias_IdPericia",
                table: "TracosRaciaisPericias",
                column: "IdPericia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonagensPericias");

            migrationBuilder.DropTable(
                name: "TracosRaciaisPericias");
        }
    }
}
