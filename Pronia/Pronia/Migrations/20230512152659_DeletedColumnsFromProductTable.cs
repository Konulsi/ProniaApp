using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pronia.Migrations
{
    public partial class DeletedColumnsFromProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoverImage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "IsHover",
                table: "ProductImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "ProductImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHover",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "HoverImage",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
