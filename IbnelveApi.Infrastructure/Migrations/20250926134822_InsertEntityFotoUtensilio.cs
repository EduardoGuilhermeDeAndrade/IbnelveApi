using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbnelveApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertEntityFotoUtensilio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utensilios_Utensilios_UtensilioId",
                table: "Utensilios");

            migrationBuilder.DropIndex(
                name: "IX_Utensilios_UtensilioId",
                table: "Utensilios");

            migrationBuilder.DropColumn(
                name: "UtensilioId",
                table: "Utensilios");

            migrationBuilder.CreateTable(
                name: "FotoUtensilio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtensilioId = table.Column<int>(type: "int", nullable: false),
                    ArquivoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrincipal = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotoUtensilio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotoUtensilio_Utensilios_UtensilioId",
                        column: x => x.UtensilioId,
                        principalTable: "Utensilios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FotoUtensilio_UtensilioId",
                table: "FotoUtensilio",
                column: "UtensilioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FotoUtensilio");

            migrationBuilder.AddColumn<int>(
                name: "UtensilioId",
                table: "Utensilios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utensilios_UtensilioId",
                table: "Utensilios",
                column: "UtensilioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utensilios_Utensilios_UtensilioId",
                table: "Utensilios",
                column: "UtensilioId",
                principalTable: "Utensilios",
                principalColumn: "Id");
        }
    }
}
