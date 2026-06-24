using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarlockProgressionAndFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaracteristicasClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdClasse = table.Column<int>(type: "integer", nullable: false),
                    Nivel = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaracteristicasClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaracteristicasClasses_Classes_IdClasse",
                        column: x => x.IdClasse,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CaracteristicasClasses",
                columns: new[] { "Id", "Descricao", "IdClasse", "Nivel", "Nome" },
                values: new object[,]
                {
                    { 1, "Seu patrono concede a você características no 1º nível e novamente no 6º, 10º e 14º nível.", 1, 1, "Patrono Transcendental" },
                    { 2, "Sua pesquisa arcana e a magia infundida em você por seu patrono concedem a você a capacidade de conjurar magias.", 1, 1, "Magia de Pacto" },
                    { 3, "Em suas investigações sobre o conhecimento oculto, você descobriu invocações místicas, fragmentos de conhecimento proibido que infundem você com uma habilidade mágica duradoura.", 1, 2, "Invocações Místicas" },
                    { 4, "No 3º nível, seu patrono transcendental concede a você um favor por seus serviços leais. Você ganha uma das características: Pacto da Corrente, Pacto da Lâmina ou Pacto do Tomo.", 1, 3, "Dádiva do Pacto" },
                    { 5, "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica.", 1, 4, "Incremento no Valor de Habilidade" },
                    { 6, "Seu Patrono Transcendental concede a você uma característica no 6º nível baseado na sua escolha de Patrono.", 1, 6, "Característica de Patrono Transcendental" },
                    { 7, "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica.", 1, 8, "Incremento no Valor de Habilidade" },
                    { 8, "Seu Patrono Transcendental concede a você uma característica no 10º nível baseado na sua escolha de Patrono.", 1, 10, "Característica de Patrono Transcendental" },
                    { 9, "Seu patrono confere a você um segredo mágico chamado de Arcana Mística. Escolha uma magia de 6º nível da lista de magias de bruxo como sua arcana mística. Você pode conjurar essa magia uma vez sem gastar um espaço de magia. Você deve terminar um descanso longo antes de poder fazer isso novamente.", 1, 11, "Arcana Mística (6° nível)" },
                    { 10, "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica.", 1, 12, "Incremento no Valor de Habilidade" },
                    { 11, "Seu patrono confere a você um segredo mágico chamado de Arcana Mística. Escolha uma magia de 7º nível da lista de magias de bruxo como sua arcana mística. Você pode conjurar essa magia uma vez sem gastar um espaço de magia. Você deve terminar um descanso longo antes de poder fazer isso novamente.", 1, 13, "Arcana Mística (7° nível)" },
                    { 12, "Seu Patrono Transcendental concede a você uma característica no 14º nível baseado na sua escolha de Patrono.", 1, 14, "Característica de Patrono Transcendental" },
                    { 13, "Seu patrono confere a você um segredo mágico chamado de Arcana Mística. Escolha uma magia de 8º nível da lista de magias de bruxo como sua arcana mística. Você pode conjurar essa magia uma vez sem gastar um espaço de magia. Você deve terminar um descanso longo antes de poder fazer isso novamente.", 1, 15, "Arcana Mística (8° nível)" },
                    { 14, "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica.", 1, 16, "Incremento no Valor de Habilidade" },
                    { 15, "Seu patrono confere a você um segredo mágico chamado de Arcana Mística. Escolha uma magia de 9º nível da lista de magias de bruxo como sua arcana mística. Você pode conjurar essa magia uma vez sem gastar um espaço de magia. Você deve terminar um descanso longo antes de poder fazer isso novamente.", 1, 17, "Arcana Mística (9° nível)" },
                    { 16, "Você pode aumentar um valor de atributo de sua escolha em 2, ou dois valores de atributo de sua escolha em 1. Como normal, você não pode aumentar um valor de atributo acima de 20 usando essa característica.", 1, 19, "Incremento no Valor de Habilidade" },
                    { 17, "No 20º nível, você pode extrair de sua reserva interna de poder místico enquanto roga ao seu patrono para recuperar espaços de magia gastos. Você pode gastar 1 minuto implorando pela ajuda do seu patrono para recuperar todos os seus espaços de magia gastos da sua característica Magia de Pacto. Você deve terminar um descanso longo antes de usar esta característica novamente.", 1, 20, "Mestre Místico" }
                });

            migrationBuilder.InsertData(
                table: "ClasseProgressoes",
                columns: new[] { "Id", "BonusProficiencia", "EspacosMagia", "IdClasse", "InvocacoesConhecidas", "MagiasConhecidas", "Nivel", "NivelMagia", "TruquesConhecidos" },
                values: new object[,]
                {
                    { 2, 2, 2, 1, 2, 3, 2, 1, 2 },
                    { 3, 2, 2, 1, 2, 4, 3, 2, 2 },
                    { 4, 2, 2, 1, 3, 5, 4, 2, 3 },
                    { 5, 3, 2, 1, 3, 6, 5, 3, 3 },
                    { 6, 3, 2, 1, 4, 7, 6, 3, 3 },
                    { 7, 3, 2, 1, 4, 8, 7, 4, 3 },
                    { 8, 3, 2, 1, 4, 9, 8, 4, 3 },
                    { 9, 4, 2, 1, 5, 10, 9, 5, 3 },
                    { 10, 4, 2, 1, 5, 10, 10, 5, 4 },
                    { 11, 4, 3, 1, 5, 11, 11, 5, 4 },
                    { 12, 4, 3, 1, 6, 11, 12, 5, 4 },
                    { 13, 5, 3, 1, 6, 12, 13, 5, 4 },
                    { 14, 5, 3, 1, 6, 12, 14, 5, 4 },
                    { 15, 5, 3, 1, 7, 13, 15, 5, 4 },
                    { 16, 5, 3, 1, 7, 13, 16, 5, 4 },
                    { 17, 6, 4, 1, 7, 14, 17, 5, 4 },
                    { 18, 6, 4, 1, 8, 14, 18, 5, 4 },
                    { 19, 6, 4, 1, 8, 15, 19, 5, 4 },
                    { 20, 6, 4, 1, 8, 15, 20, 5, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaracteristicasClasses_IdClasse",
                table: "CaracteristicasClasses",
                column: "IdClasse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaracteristicasClasses");

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ClasseProgressoes",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
