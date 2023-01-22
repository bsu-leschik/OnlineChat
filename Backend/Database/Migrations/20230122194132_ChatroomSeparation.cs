using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class ChatroomSeparation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chatroom_Users_OwnerId",
                table: "Chatroom");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chatroom_ChatroomId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chatroom_PublicChatroomId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ChatroomUser");

            migrationBuilder.DropIndex(
                name: "IX_Users_PublicChatroomId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chatroom",
                table: "Chatroom");

            migrationBuilder.DropIndex(
                name: "IX_Chatroom_OwnerId",
                table: "Chatroom");

            migrationBuilder.DropColumn(
                name: "PublicChatroomId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Chatroom");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Chatroom");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "Chatroom",
                newName: "PublicChatroom");

            migrationBuilder.RenameColumn(
                name: "ChatroomId",
                table: "Messages",
                newName: "PublicChatroomId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ChatroomId",
                table: "Messages",
                newName: "IX_Messages_PublicChatroomId");

            migrationBuilder.AddColumn<Guid>(
                name: "PrivateChatroomId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PublicChatroom",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublicChatroom",
                table: "PublicChatroom",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PrivateChatroom",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastMessageTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateChatroom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublicChatroomUser",
                columns: table => new
                {
                    PublicChatroomsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicChatroomUser", x => new { x.PublicChatroomsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PublicChatroomUser_PublicChatroom_PublicChatroomsId",
                        column: x => x.PublicChatroomsId,
                        principalTable: "PublicChatroom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicChatroomUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivateChatroomUser",
                columns: table => new
                {
                    PrivateChatroomsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateChatroomUser", x => new { x.PrivateChatroomsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PrivateChatroomUser_PrivateChatroom_PrivateChatroomsId",
                        column: x => x.PrivateChatroomsId,
                        principalTable: "PrivateChatroom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivateChatroomUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_PrivateChatroomId",
                table: "Messages",
                column: "PrivateChatroomId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChatroomUser_UsersId",
                table: "PrivateChatroomUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicChatroomUser_UsersId",
                table: "PublicChatroomUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_PrivateChatroom_PrivateChatroomId",
                table: "Messages",
                column: "PrivateChatroomId",
                principalTable: "PrivateChatroom",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_PublicChatroom_PublicChatroomId",
                table: "Messages",
                column: "PublicChatroomId",
                principalTable: "PublicChatroom",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_PrivateChatroom_PrivateChatroomId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_PublicChatroom_PublicChatroomId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "PrivateChatroomUser");

            migrationBuilder.DropTable(
                name: "PublicChatroomUser");

            migrationBuilder.DropTable(
                name: "PrivateChatroom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_PrivateChatroomId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublicChatroom",
                table: "PublicChatroom");

            migrationBuilder.DropColumn(
                name: "PrivateChatroomId",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameTable(
                name: "PublicChatroom",
                newName: "Chatroom");

            migrationBuilder.RenameColumn(
                name: "PublicChatroomId",
                table: "Message",
                newName: "ChatroomId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_PublicChatroomId",
                table: "Message",
                newName: "IX_Message_ChatroomId");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicChatroomId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Chatroom",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Chatroom",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Chatroom",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chatroom",
                table: "Chatroom",
                column: "Id");

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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatroomUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PublicChatroomId",
                table: "Users",
                column: "PublicChatroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Chatroom_OwnerId",
                table: "Chatroom",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatroomUser_UsersId",
                table: "ChatroomUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chatroom_Users_OwnerId",
                table: "Chatroom",
                column: "OwnerId",
                principalTable: "Users",
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
    }
}
