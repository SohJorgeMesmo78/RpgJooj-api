using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterHealthFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VidaAtual",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VidaMaxima",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ClassesPersonagens",
                keyColumns: new[] { "IdClasse", "IdPersonagem" },
                keyValues: new object[] { 1, 1 },
                column: "Nivel",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Personagens",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "VidaAtual", "VidaMaxima" },
                values: new object[] { 9, 9 });

            migrationBuilder.UpdateData(
                table: "Personagens",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "VidaAtual", "VidaMaxima" },
                values: new object[] { 38, 38 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VidaAtual",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "VidaMaxima",
                table: "Personagens");

            migrationBuilder.UpdateData(
                table: "ClassesPersonagens",
                keyColumns: new[] { "IdClasse", "IdPersonagem" },
                keyValues: new object[] { 1, 1 },
                column: "Nivel",
                value: 3);
        }
    }
}
