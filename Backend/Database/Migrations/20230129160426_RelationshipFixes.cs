using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Users_OwnerId",
                table: "Administrators");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Users_OwnerId",
                table: "Administrators",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Users_OwnerId",
                table: "Administrators");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Users_OwnerId",
                table: "Administrators",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
