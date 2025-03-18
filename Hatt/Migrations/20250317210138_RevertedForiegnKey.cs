using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hatt.Migrations
{
    /// <inheritdoc />
    public partial class RevertedForiegnKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationsUsers_Users_UserName",
                table: "ConversationsUsers");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "ConversationsUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ConversationsUsers_UserName",
                table: "ConversationsUsers",
                newName: "IX_ConversationsUsers_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationsUsers_Users_UserId",
                table: "ConversationsUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationsUsers_Users_UserId",
                table: "ConversationsUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ConversationsUsers",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_ConversationsUsers_UserId",
                table: "ConversationsUsers",
                newName: "IX_ConversationsUsers_UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationsUsers_Users_UserName",
                table: "ConversationsUsers",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
