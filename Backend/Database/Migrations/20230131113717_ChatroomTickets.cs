using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class ChatroomTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatroomUser");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatroomId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatroomTicket",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatroomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastMessageRead = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatroomTicket", x => new { x.UserId, x.ChatroomId });
                    table.ForeignKey(
                        name: "FK_ChatroomTicket_Chatroom_ChatroomId",
                        column: x => x.ChatroomId,
                        principalTable: "Chatroom",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatroomTicket_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChatroomId",
                table: "Users",
                column: "ChatroomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatroomTicket_ChatroomId",
                table: "ChatroomTicket",
                column: "ChatroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chatroom_ChatroomId",
                table: "Users",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chatroom_ChatroomId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ChatroomTicket");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChatroomId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatroomId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "ChatroomUser",
                columns: table => new
                {
                    ChatroomsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatroomUser", x => new { x.ChatroomsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ChatroomUser_Chatroom_ChatroomsId",
                        column: x => x.ChatroomsId,
                        principalTable: "Chatroom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatroomUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatroomUser_UsersId",
                table: "ChatroomUser",
                column: "UsersId");
        }
    }
}
