namespace WebApplicationCQRS.Application.DTOs;

public class AssignedTicketDto
{
    public int TicketId { get; set; }

    public int AssigneeId { get; set; } // Người được giao ticket

    public AssignedTicketDto()
    {
    }

    public AssignedTicketDto(int ticketId, int assigneeId)
    {
        TicketId = ticketId;
        AssigneeId = assigneeId;
    }
}