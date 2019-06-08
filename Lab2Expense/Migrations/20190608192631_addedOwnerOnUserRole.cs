using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab2Expense.Migrations
{
    public partial class addedOwnerOnUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "UserRole",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_OwnerId",
                table: "UserRole",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Users_OwnerId",
                table: "UserRole",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Users_OwnerId",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_OwnerId",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "UserRole");
        }
    }
}
