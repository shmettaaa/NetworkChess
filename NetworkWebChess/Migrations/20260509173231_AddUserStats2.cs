using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkWebChess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStats2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Draws",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GamesPlayed",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Losses",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Draws",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GamesPlayed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "Users");
        }
    }
}
