using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hatt.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMessageField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sender_id",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "SenderUsername",
                table: "Messages",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderUsername",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "Sender_id",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
