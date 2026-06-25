using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    Peso = table.Column<double>(type: "double precision", nullable: true),
                    ProficienciaRequerida = table.Column<string>(type: "text", nullable: true),
                    TipoEquipamento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Propriedades = table.Column<List<string>>(type: "text[]", nullable: false),
                    Dano = table.Column<string>(type: "text", nullable: true),
                    TipoDano = table.Column<string>(type: "text", nullable: true),
                    ModificadorClasseArmadura = table.Column<int>(type: "integer", nullable: true),
                    ClasseArmadura = table.Column<int>(type: "integer", nullable: true),
                    PermiteDestreza = table.Column<bool>(type: "boolean", nullable: true),
                    ForcaRequerida = table.Column<int>(type: "integer", nullable: true),
                    DesvantagemFurtividade = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonagensEquipamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersonagem = table.Column<int>(type: "integer", nullable: false),
                    IdEquipamento = table.Column<int>(type: "integer", nullable: false),
                    IsEquipado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonagensEquipamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonagensEquipamentos_Equipamentos_IdEquipamento",
                        column: x => x.IdEquipamento,
                        principalTable: "Equipamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonagensEquipamentos_Personagens_IdPersonagem",
                        column: x => x.IdPersonagem,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Equipamentos",
                columns: new[] { "Id", "ClasseArmadura", "Dano", "Descricao", "DesvantagemFurtividade", "ForcaRequerida", "ModificadorClasseArmadura", "Nome", "PermiteDestreza", "Peso", "ProficienciaRequerida", "Propriedades", "TipoDano", "TipoEquipamento" },
                values: new object[,]
                {
                    { 1, null, null, "Um belo alaúde de madeira polida, usado por bardos e músicos.", null, null, null, "Alaúde", null, 1.0, null, new List<string>(), null, "Outro" },
                    { 2, null, null, "Uma bolsa de couro impermeável com todos os componentes materiais necessários para conjurar suas magias.", null, null, null, "Bolsa de componentes", null, 1.0, null, new List<string>(), null, "Outro" },
                    { 3, null, "1d4", null, null, null, null, "Adaga", null, 0.5, "Armas simples", new List<string> { "Acuidade", "Leve", "Arremesso" }, "perfurante", "Arma" },
                    { 4, null, "1d8", null, null, null, null, "Clava Grande", null, 5.0, "Armas simples", new List<string> { "Duas mãos", "Pesada" }, "concussão", "Arma" },
                    { 5, null, null, "Um escudo de metal gravado com o brasão do reino, adiciona +2 à Classe de Armadura (CA).", null, null, 2, "Escudo", null, 3.0, "Escudos", new List<string>(), null, "Escudo" },
                    { 6, 12, null, "Uma armadura feita de couro resistente, reforçado com rebites de metal. Fornece CA 12 + Modificador de Destreza.", false, 0, null, "Armadura de Couro Batido", true, 6.5, "Armaduras leves", new List<string>(), null, "Armadura" }
                });

            migrationBuilder.InsertData(
                table: "PersonagensEquipamentos",
                columns: new[] { "Id", "IdEquipamento", "IdPersonagem", "IsEquipado" },
                values: new object[,]
                {
                    { 1, 1, 1, false },
                    { 2, 2, 1, false },
                    { 3, 3, 1, false },
                    { 4, 3, 1, false },
                    { 5, 4, 1, false },
                    { 6, 5, 1, false },
                    { 7, 6, 1, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonagensEquipamentos_IdEquipamento",
                table: "PersonagensEquipamentos",
                column: "IdEquipamento");

            migrationBuilder.CreateIndex(
                name: "IX_PersonagensEquipamentos_IdPersonagem",
                table: "PersonagensEquipamentos",
                column: "IdPersonagem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonagensEquipamentos");

            migrationBuilder.DropTable(
                name: "Equipamentos");
        }
    }
}
