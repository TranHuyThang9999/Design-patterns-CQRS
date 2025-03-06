using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplicationCQRS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssignedTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssignedTickets_AssigneeId",
                table: "AssignedTickets");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTickets_AssigneeId_TicketId",
                table: "AssignedTickets",
                columns: new[] { "AssigneeId", "TicketId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssignedTickets_AssigneeId_TicketId",
                table: "AssignedTickets");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTickets_AssigneeId",
                table: "AssignedTickets",
                column: "AssigneeId");
        }
    }
}
