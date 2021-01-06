using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Migrations
{
    public partial class v17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Item_SprintId",
                table: "Item",
                column: "SprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Backlog_SprintId",
                table: "Item",
                column: "SprintId",
                principalTable: "Backlog",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Backlog_SprintId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_SprintId",
                table: "Item");
        }
    }
}
