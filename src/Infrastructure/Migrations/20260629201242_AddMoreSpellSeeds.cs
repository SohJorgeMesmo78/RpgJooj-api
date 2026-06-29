using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSpellSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 7,
                column: "Propriedades",
                value: new List<string>());

            migrationBuilder.InsertData(
                table: "Magias",
                columns: new[] { "Id", "Descricao", "Nivel", "Nome" },
                values: new object[,]
                {
                    { 2, "Você cria um som ou uma imagem de um objeto dentro do alcance, que permanece pela duração.", 0, "Ilusão Menor" },
                    { 3, "Você aponta seu dedo para uma criatura dentro do alcance e sussurra uma mensagem. O alvo ouve a mensagem e pode responder com um sussurro.", 0, "Mensagem" },
                    { 4, "Uma mão espectral flutuante aparece em um ponto à sua escolha, dentro do alcance. A mão permanece pela duração ou até você dispensá-la.", 0, "Mãos mágicas" },
                    { 5, "Este truque é um truque mágico simples que conjuradores iniciantes usam para praticar.", 0, "Prestidigitação" },
                    { 6, "Você tem vantagem em todos os testes de Carisma direcionados a uma criatura à sua escolha que não seja hostil a você.", 0, "Amizade" },
                    { 7, "Você roga uma maldição em uma criatura dentro do alcance. Até a magia acabar, você causa 1d6 de dano necrótico extra ao alvo sempre que acertá-lo com um ataque.", 1, "Bruxaria (Hex)" },
                    { 8, "Pela duração, você sente a presença de magia a até 9 metros de você. Se você sentir magia dessa forma, você pode usar sua ação para ver uma aura tênue ao redor de qualquer criatura ou objeto visível na área que possua magia.", 1, "Detectar Magia" },
                    { 9, "Uma onda de força trovejante irrompe a partir de você. Cada criatura num cubo de 4,5 metros centrado em você deve realizar um teste de resistência de Constituição.", 1, "Onda de Trovão" },
                    { 10, "Você conjura um espírito que assume a forma de um animal de sua escolha: morcego, gato, caranguejo, sapo, falcão, lagarto, polvo, coruja, rato, corvo, cavalo-marinho, aranha ou doninha.", 1, "Encontrar Familiar" },
                    { 11, "Brevemente envolto por uma fumaça prateada, você se teletransporta a até 9 metros para um local desocupado que você possa ver.", 2, "Passo Nebuloso" },
                    { 12, "Você sugere um curso de atividade (limitado a uma frase ou duas) e magicamente influencia uma criatura que você possa ver dentro do alcance e que possa ouvir e compreender você.", 2, "Sugestão" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Magias",
                keyColumn: "Id",
                keyValue: 12);

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
                table: "Equipamentos",
                keyColumn: "Id",
                keyValue: 7,
                column: "Propriedades",
                value: new List<string>());
        }
    }
}
