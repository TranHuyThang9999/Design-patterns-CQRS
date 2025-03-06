namespace WebApplicationCQRS.Application.DTOs;

public class AssignedTicketDto
{
    public int TicketId { get; set; }

    public int AssigneeId { get; set; } // Người được giao ticket

    public int AssignerId { get; set; } // Người giao ticket
    
    public AssignedTicketDto(){}
    public AssignedTicketDto(int ticketId, int assigneeId, int assignerId)
    {
        TicketId = ticketId;
        AssigneeId = assigneeId;
        AssignerId = assignerId;
    }
}