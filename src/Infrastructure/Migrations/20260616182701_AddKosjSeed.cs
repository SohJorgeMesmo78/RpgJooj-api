using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKosjSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Personagens",
                columns: new[] { "Id", "Base64Imagem", "Codigo", "Nome" },
                values: new object[] { 2, null, "kosj", "Kosj" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Personagens",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
