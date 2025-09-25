using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbnelveApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateEntityCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Utensilios",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "CategoriaTarefas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "CategoriaTarefas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "CategoriaTarefas",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cor",
                table: "CategoriaTarefas",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Ativa",
                table: "CategoriaTarefas",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ativa = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utensilios_CategoriaId",
                table: "Utensilios",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaTarefas_CreatedAt",
                table: "CategoriaTarefas",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaTarefas_Nome_TenantId",
                table: "CategoriaTarefas",
                columns: new[] { "Nome", "TenantId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaTarefas_TenantId_Ativa",
                table: "CategoriaTarefas",
                columns: new[] { "TenantId", "Ativa" });

            migrationBuilder.AddForeignKey(
                name: "FK_Utensilios_Categoria_CategoriaId",
                table: "Utensilios",
                column: "CategoriaId",
                principalTable: "Categoria",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utensilios_Categoria_CategoriaId",
                table: "Utensilios");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropIndex(
                name: "IX_Utensilios_CategoriaId",
                table: "Utensilios");

            migrationBuilder.DropIndex(
                name: "IX_CategoriaTarefas_CreatedAt",
                table: "CategoriaTarefas");

            migrationBuilder.DropIndex(
                name: "IX_CategoriaTarefas_Nome_TenantId",
                table: "CategoriaTarefas");

            migrationBuilder.DropIndex(
                name: "IX_CategoriaTarefas_TenantId_Ativa",
                table: "CategoriaTarefas");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Utensilios");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "CategoriaTarefas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "CategoriaTarefas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "CategoriaTarefas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cor",
                table: "CategoriaTarefas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(7)",
                oldMaxLength: 7,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Ativa",
                table: "CategoriaTarefas",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
