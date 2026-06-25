using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoneyAndEquipPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PersonagensEquipamentos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.AddColumn<int>(
                name: "PecaCobre",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PecaElectro",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PecaOuro",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PecaPlatina",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PecaPrata",
                table: "Personagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Preco",
                table: "Equipamentos",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Preco", "Propriedades" },
                values: new object[] { "35 PO", new List<string>() });

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Preco", "Propriedades" },
                values: new object[] { "25 PO", new List<string>() });

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Preco", "Propriedades" },
                values: new object[] { "2 PO", new List<string> { "Acuidade", "Leve", "Arremesso" } });

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Preco", "Propriedades" },
                values: new object[] { "2 PP", new List<string> { "Duas mãos", "Pesada" } });

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Preco", "Propriedades" },
                values: new object[] { "10 PO", new List<string>() });

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Preco", "Propriedades" },
                values: new object[] { "45 PO", new List<string>() });

            migrationBuilder.InsertData(
                table: "Equipamentos",
                columns: new[] { "Id", "ClasseArmadura", "Dano", "Descricao", "DesvantagemFurtividade", "ForcaRequerida", "ModificadorClasseArmadura", "Nome", "PermiteDestreza", "Peso", "Preco", "ProficienciaRequerida", "Propriedades", "TipoDano", "TipoEquipamento" },
                values: new object[] { 7, null, null, "Um conjunto de roupas robustas composto por botas, calças ou saia de lã, um cinto, uma túnica quente e um manto com capuz.", null, null, null, "Roupas de viajante", null, 1.8, "2 PO", null, new List<string>(), null, "Outro" });

            migrationBuilder.UpdateData(
                table: "Personagens",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PecaCobre", "PecaElectro", "PecaOuro", "PecaPlatina", "PecaPrata" },
                values: new object[] { 0, 0, 57, 0, 0 });

            migrationBuilder.UpdateData(
                table: "PersonagensEquipamentos",
                keyColumn: "Id",
                keyValue: 4,
                column: "IdEquipamento",
                value: 5);

            migrationBuilder.UpdateData(
                table: "PersonagensEquipamentos",
                keyColumn: "Id",
                keyValue: 5,
                column: "IdEquipamento",
                value: 6);

            migrationBuilder.UpdateData(
                table: "PersonagensEquipamentos",
                keyColumn: "Id",
                keyValue: 6,
                column: "IdEquipamento",
                value: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "PecaCobre",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "PecaElectro",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "PecaOuro",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "PecaPlatina",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "PecaPrata",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Preco",
                table: "Equipamentos");

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 1,
                column: "Propriedades",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 2,
                column: "Propriedades",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 3,
                column: "Propriedades",
                value: new List<string> { "Acuidade", "Leve", "Arremesso" });

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 4,
                column: "Propriedades",
                value: new List<string> { "Duas mãos", "Pesada" });

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 5,
                column: "Propriedades",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 6,
                column: "Propriedades",
                value: new List<string>());

            migrationBuilder.UpdateData(
                table: "PersonagensEquipamentos",
                keyColumn: "Id",
                keyValue: 4,
                column: "IdEquipamento",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PersonagensEquipamentos",
                keyColumn: "Id",
                keyValue: 5,
                column: "IdEquipamento",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PersonagensEquipamentos",
                keyColumn: "Id",
                keyValue: 6,
                column: "IdEquipamento",
                value: 5);

            migrationBuilder.InsertData(
                table: "PersonagensEquipamentos",
                columns: new[] { "Id", "IdEquipamento", "IdPersonagem", "IsEquipado" },
                values: new object[] { 7, 6, 1, false });
        }
    }
}
