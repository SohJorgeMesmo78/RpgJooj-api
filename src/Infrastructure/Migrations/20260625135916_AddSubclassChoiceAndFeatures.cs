using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubclassChoiceAndFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubclasseEscolha",
                table: "ClassesPersonagens",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "CaracteristicasClasses",
                columns: new[] { "Id", "Descricao", "IdClasse", "Nivel", "Nome" },
                values: new object[,]
                {
                    { 18, "Com uma ação, você pode desaparecer magicamente e entrar em seu receptáculo, que permanece no espaço que você deixou. O interior do receptáculo é um espaço extradimensional na forma de um cilindro de 6m de raio e 6m de altura, e lembra o objeto. O interior é mobiliado com almofadas e mesas baixas e fica em uma temperatura confortável. Enquanto dentro, você pode ouvir a área ao redor do receptáculo como se estivesse em seu espaço. Você pode permanecer dentro do receptáculo por uma quantia de horas igual a duas vezes seu bônus de proficiência. Você pode sair do receptáculo antes desse tempo se usar uma ação bônus para isso, se você morrer, ou se o receptáculo for destruído. Quando você sai desse receptáculo, você aparece no espaço desocupado mais próximo dele. Qualquer objeto deixado no receptáculo permanece lá até ser carregado para fora, e se o receptáculo for destruído, todo objeto contido por ele reaparece sem danos nos espaços desocupados mais próximos à antiga localização do receptáculo. Uma vez que entre no receptáculo, você não pode fazer isso novamente até terminar um descanso longo.\n\nA CA do receptáculo é igual à sua CD de magia. Ele tem pontos de vida igual ao seu nível de bruxo mais seu bônus de proficiência, e ele é imune a dano venenoso e psíquico.\nSe o receptáculo for destruído, você pode realizar uma cerimônia de 1 hora para receber um substituto de seu patrono. Essa cerimônia pode ser realizada durante um descanso curto ou longo, e o receptáculo anterior é destruído caso ainda exista. O receptáculo desaparece em um clarão de poder elemental quando você morre.", 3, 1, "Descanso Engarrafado" },
                    { 19, "Uma vez por turno, quando você acerta uma rolagem de ataque, você pode causar dano extra ao alvo igual ao seu bônus de proficiência. O tipo desse dano é determinado por seu patrono: contundente (dao), trovejante (djinni), ígneo (ifriti), ou gélido (marid).", 3, 1, "Ira do Gênio" }
                });

            migrationBuilder.UpdateData(
                table: "ClassesPersonagens",
                keyColumns: new[] { "IdClasse", "IdPersonagem" },
                keyValues: new object[] { 1, 1 },
                column: "SubclasseEscolha",
                value: "Djinni");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CaracteristicasClasses",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CaracteristicasClasses",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DropColumn(
                name: "SubclasseEscolha",
                table: "ClassesPersonagens");

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
