using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eplace.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoORetornoDnv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Produtos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Imagem",
                table: "Produtos",
                type: "longblob",
                nullable: false);
        }
    }
}
