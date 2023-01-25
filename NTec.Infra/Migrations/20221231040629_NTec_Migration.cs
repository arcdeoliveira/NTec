using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTec.Infra.Migrations
{
    /// <inheritdoc />
    public partial class NTecMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cargos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AlteradoPor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DataDeAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDeCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2022, 12, 31, 1, 6, 28, 882, DateTimeKind.Local).AddTicks(3583)),
                    DataDeExclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Excluido = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExcluidoPor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "setores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AlteradoPor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DataDeAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDeCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2022, 12, 31, 1, 6, 28, 883, DateTimeKind.Local).AddTicks(673)),
                    DataDeExclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Excluido = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExcluidoPor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_setores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "colaboradores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cpf = table.Column<long>(type: "bigint", fixedLength: true, maxLength: 11, nullable: false),
                    Genero = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Excluido = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ExcluidoPor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DataDeExclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AlteradoPor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DataDeAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Aniversario = table.Column<DateTime>(type: "date", nullable: false),
                    DataDeCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2022, 12, 31, 1, 6, 28, 882, DateTimeKind.Local).AddTicks(5445)),
                    Foto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SobreNome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CargoId = table.Column<int>(type: "int", nullable: false),
                    ChefeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SetorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colaboradores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_colaboradores_cargos_CargoId",
                        column: x => x.CargoId,
                        principalTable: "cargos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_colaboradores_colaboradores_ChefeId",
                        column: x => x.ChefeId,
                        principalTable: "colaboradores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_colaboradores_setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_colaboradores_CargoId",
                table: "colaboradores",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_colaboradores_ChefeId",
                table: "colaboradores",
                column: "ChefeId");

            migrationBuilder.CreateIndex(
                name: "IX_colaboradores_SetorId",
                table: "colaboradores",
                column: "SetorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "colaboradores");

            migrationBuilder.DropTable(
                name: "cargos");

            migrationBuilder.DropTable(
                name: "setores");
        }
    }
}
