using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbnelveApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertTaskManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Prioridade = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_CreatedAt",
                table: "Tarefas",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_IsDeleted",
                table: "Tarefas",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_TenantId",
                table: "Tarefas",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_TenantId_Categoria",
                table: "Tarefas",
                columns: new[] { "TenantId", "Categoria" });

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_TenantId_DataVencimento",
                table: "Tarefas",
                columns: new[] { "TenantId", "DataVencimento" });

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_TenantId_Prioridade",
                table: "Tarefas",
                columns: new[] { "TenantId", "Prioridade" });

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_TenantId_Status",
                table: "Tarefas",
                columns: new[] { "TenantId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarefas");
        }
    }
}
