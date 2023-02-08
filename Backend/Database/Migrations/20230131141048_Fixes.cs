using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomTicket_Chatroom_ChatroomId",
                table: "ChatroomTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomTicket_Users_UserId",
                table: "ChatroomTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chatroom_ChatroomId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChatroomId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatroomId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatroomTicket_Chatroom_ChatroomId",
                table: "ChatroomTicket",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatroomTicket_Users_UserId",
                table: "ChatroomTicket",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomTicket_Chatroom_ChatroomId",
                table: "ChatroomTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomTicket_Users_UserId",
                table: "ChatroomTicket");

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
                name: "FK_ChatroomTicket_Chatroom_ChatroomId",
                table: "ChatroomTicket",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatroomTicket_Users_UserId",
                table: "ChatroomTicket",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chatroom_ChatroomId",
                table: "Users",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id");
        }
    }
}
