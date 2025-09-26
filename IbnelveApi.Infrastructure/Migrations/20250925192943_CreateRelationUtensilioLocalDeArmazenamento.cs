using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbnelveApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelationUtensilioLocalDeArmazenamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utensilios_CategoriaUtensilios_CategoriaId",
                table: "Utensilios");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Utensilios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroSerie",
                table: "Utensilios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NomeFornecedor",
                table: "Utensilios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Utensilios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocalDeArmazenamentoId",
                table: "Utensilios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UtensilioId",
                table: "Utensilios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocaisDeArmazenamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContatoResponsavel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Ativa = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocaisDeArmazenamento", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utensilios_LocalDeArmazenamentoId",
                table: "Utensilios",
                column: "LocalDeArmazenamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Utensilios_UtensilioId",
                table: "Utensilios",
                column: "UtensilioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utensilios_CategoriaUtensilios_CategoriaId",
                table: "Utensilios",
                column: "CategoriaId",
                principalTable: "CategoriaUtensilios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utensilios_LocaisDeArmazenamento_LocalDeArmazenamentoId",
                table: "Utensilios",
                column: "LocalDeArmazenamentoId",
                principalTable: "LocaisDeArmazenamento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utensilios_Utensilios_UtensilioId",
                table: "Utensilios",
                column: "UtensilioId",
                principalTable: "Utensilios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utensilios_CategoriaUtensilios_CategoriaId",
                table: "Utensilios");

            migrationBuilder.DropForeignKey(
                name: "FK_Utensilios_LocaisDeArmazenamento_LocalDeArmazenamentoId",
                table: "Utensilios");

            migrationBuilder.DropForeignKey(
                name: "FK_Utensilios_Utensilios_UtensilioId",
                table: "Utensilios");

            migrationBuilder.DropTable(
                name: "LocaisDeArmazenamento");

            migrationBuilder.DropIndex(
                name: "IX_Utensilios_LocalDeArmazenamentoId",
                table: "Utensilios");

            migrationBuilder.DropIndex(
                name: "IX_Utensilios_UtensilioId",
                table: "Utensilios");

            migrationBuilder.DropColumn(
                name: "LocalDeArmazenamentoId",
                table: "Utensilios");

            migrationBuilder.DropColumn(
                name: "UtensilioId",
                table: "Utensilios");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Utensilios",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumeroSerie",
                table: "Utensilios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NomeFornecedor",
                table: "Utensilios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Utensilios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Utensilios_CategoriaUtensilios_CategoriaId",
                table: "Utensilios",
                column: "CategoriaId",
                principalTable: "CategoriaUtensilios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
