using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterDetailsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alinhamento",
                table: "Personagens",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Classe",
                table: "Personagens",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Nivel",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Raca",
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
                columns: new[] { "Alinhamento", "Classe", "Nivel", "Raca", "Subclasse" },
                values: new object[] { "Caótico e Bom", "Bruxo", 3, "Eladryn", "o Gênio" });

            migrationBuilder.UpdateData(
                table: "Personagens",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Alinhamento", "Classe", "Nivel", "Raca", "Subclasse" },
                values: new object[] { "Leal e Neutro", "Monge", 5, "Homem-Lagarto", "Caminho do eu astral" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alinhamento",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Classe",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Raca",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Subclasse",
                table: "Personagens");
        }
    }
}
