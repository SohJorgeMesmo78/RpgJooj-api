using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddActionTooltips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcertoTooltip",
                table: "AcoesPersonagens",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DanoTooltip",
                table: "AcoesPersonagens",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AcoesPersonagens",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AcertoTooltip", "DanoTooltip" },
                values: new object[] { "[FOR](FOR) + [PROF](Proficiência)", "1 (Base) + [FOR](FOR)" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcertoTooltip",
                table: "AcoesPersonagens");

            migrationBuilder.DropColumn(
                name: "DanoTooltip",
                table: "AcoesPersonagens");
        }
    }
}
