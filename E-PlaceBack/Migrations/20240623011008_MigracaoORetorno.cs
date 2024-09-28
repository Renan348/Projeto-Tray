using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eplace.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoORetorno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Produto",
                table: "Produtos");

            migrationBuilder.AddColumn<bool>(
                name: "AliExpress",
                table: "Produtos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MercadoLivre",
                table: "Produtos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Shopee",
                table: "Produtos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AliExpress",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "MercadoLivre",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Shopee",
                table: "Produtos");

            migrationBuilder.AddColumn<string>(
                name: "Produto",
                table: "Produtos",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
