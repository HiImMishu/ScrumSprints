using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Migrations
{
    public partial class v171 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Product_DevTeam",
                table: "Product",
                column: "DevTeam");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Team_DevTeam",
                table: "Product",
                column: "DevTeam",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Team_DevTeam",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_DevTeam",
                table: "Product");
        }
    }
}
