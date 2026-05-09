using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkWebChess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WhitePlayerId = table.Column<Guid>(type: "uuid", nullable: true),
                    BlackPlayerId = table.Column<Guid>(type: "uuid", nullable: true),
                    WhitePlayerNickname = table.Column<string>(type: "text", nullable: true),
                    BlackPlayerNickname = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    GameResult = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastActivityUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrentFen = table.Column<string>(type: "text", nullable: false),
                    CurrentPlayer = table.Column<string>(type: "text", nullable: false),
                    WhiteKingMoved = table.Column<bool>(type: "boolean", nullable: false),
                    BlackKingMoved = table.Column<bool>(type: "boolean", nullable: false),
                    WhiteKingsideRookMoved = table.Column<bool>(type: "boolean", nullable: false),
                    WhiteQueensideRookMoved = table.Column<bool>(type: "boolean", nullable: false),
                    BlackKingsideRookMoved = table.Column<bool>(type: "boolean", nullable: false),
                    BlackQueensideRookMoved = table.Column<bool>(type: "boolean", nullable: false),
                    EnPassantTarget = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    SessionToken = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nickname",
                table: "Users",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SessionToken",
                table: "Users",
                column: "SessionToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
