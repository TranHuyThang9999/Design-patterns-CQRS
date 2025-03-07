using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplicationCQRS.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToAssignedTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssignedTickets_AssigneeId_TicketId",
                table: "AssignedTickets");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AssignedTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTickets_AssigneeId_TicketId_Status",
                table: "AssignedTickets",
                columns: new[] { "AssigneeId", "TicketId", "Status" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssignedTickets_AssigneeId_TicketId_Status",
                table: "AssignedTickets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AssignedTickets");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTickets_AssigneeId_TicketId",
                table: "AssignedTickets",
                columns: new[] { "AssigneeId", "TicketId" },
                unique: true);
        }
    }
}
