using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class FixedChatroomToTicketRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomTicket_Chatroom_ChatroomId",
                table: "ChatroomTicket");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatroomTicket_Chatroom_ChatroomId",
                table: "ChatroomTicket",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomTicket_Chatroom_ChatroomId",
                table: "ChatroomTicket");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatroomTicket_Chatroom_ChatroomId",
                table: "ChatroomTicket",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id");
        }
    }
}
