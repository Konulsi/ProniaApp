using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pronia.Migrations
{
    public partial class AddColumnForBlogImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_Products_ProductId",
                table: "BlogComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_ProductId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BlogComments");

            migrationBuilder.AddColumn<bool>(
                name: "IsHover",
                table: "BlogImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "BlogImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHover",
                table: "BlogImages");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "BlogImages");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "BlogComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_ProductId",
                table: "BlogComments",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_Products_ProductId",
                table: "BlogComments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
