using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplicationCQRS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssignedTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedTickets_Users_UserId",
                table: "AssignedTickets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AssignedTickets",
                newName: "AssignerId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignedTickets_UserId",
                table: "AssignedTickets",
                newName: "IX_AssignedTickets_AssignerId");

            migrationBuilder.AddColumn<int>(
                name: "AssigneeId",
                table: "AssignedTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTickets_AssigneeId",
                table: "AssignedTickets",
                column: "AssigneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedTickets_Users_AssigneeId",
                table: "AssignedTickets",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedTickets_Users_AssignerId",
                table: "AssignedTickets",
                column: "AssignerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedTickets_Users_AssigneeId",
                table: "AssignedTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignedTickets_Users_AssignerId",
                table: "AssignedTickets");

            migrationBuilder.DropIndex(
                name: "IX_AssignedTickets_AssigneeId",
                table: "AssignedTickets");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "AssignedTickets");

            migrationBuilder.RenameColumn(
                name: "AssignerId",
                table: "AssignedTickets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignedTickets_AssignerId",
                table: "AssignedTickets",
                newName: "IX_AssignedTickets_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedTickets_Users_UserId",
                table: "AssignedTickets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
