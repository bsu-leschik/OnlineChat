using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class ChatroomHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomUser_Chatrooms_ChatroomsId",
                table: "ChatroomUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chatrooms_ChatroomId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "Chatrooms");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicChatroomId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Chatroom",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastMessageTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chatroom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chatroom_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PublicChatroomId",
                table: "Users",
                column: "PublicChatroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Chatroom_OwnerId",
                table: "Chatroom",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatroomUser_Chatroom_ChatroomsId",
                table: "ChatroomUser",
                column: "ChatroomsId",
                principalTable: "Chatroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Chatroom_ChatroomId",
                table: "Message",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chatroom_PublicChatroomId",
                table: "Users",
                column: "PublicChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomUser_Chatroom_ChatroomsId",
                table: "ChatroomUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chatroom_ChatroomId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chatroom_PublicChatroomId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Chatroom");

            migrationBuilder.DropIndex(
                name: "IX_Users_PublicChatroomId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PublicChatroomId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Chatrooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chatrooms", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatroomUser_Chatrooms_ChatroomsId",
                table: "ChatroomUser",
                column: "ChatroomsId",
                principalTable: "Chatrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Chatrooms_ChatroomId",
                table: "Message",
                column: "ChatroomId",
                principalTable: "Chatrooms",
                principalColumn: "Id");
        }
    }
}
