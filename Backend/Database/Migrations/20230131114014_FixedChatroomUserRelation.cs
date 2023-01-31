using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class FixedChatroomUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chatroom_ChatroomId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChatroomId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatroomId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChatroomId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChatroomId",
                table: "Users",
                column: "ChatroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chatroom_ChatroomId",
                table: "Users",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id");
        }
    }
}
