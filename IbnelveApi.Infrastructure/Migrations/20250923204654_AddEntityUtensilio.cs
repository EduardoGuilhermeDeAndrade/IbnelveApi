using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IbnelveApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityUtensilio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Endereco_Pais",
                table: "Membros",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco_Pais",
                table: "Membros");
        }
    }
}
