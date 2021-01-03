using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Migrations
{
    public partial class v16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Backlog_ProductId",
                table: "Backlog",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Backlog_Product_ProductId",
                table: "Backlog",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Backlog_Product_ProductId",
                table: "Backlog");

            migrationBuilder.DropIndex(
                name: "IX_Backlog_ProductId",
                table: "Backlog");
        }
    }
}
