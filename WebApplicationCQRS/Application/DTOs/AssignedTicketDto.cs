namespace WebApplicationCQRS.Application.DTOs;

public class AssignedTicketDto
{
    public List<int> TicketIds { get; set; }

    public List<int> AssigneeIds { get; set; } // Người được giao ticket

    public AssignedTicketDto()
    {
    }

    public AssignedTicketDto(List<int> ticketIds, List<int> assigneeIds)
    {
        TicketIds = ticketIds;
        AssigneeIds = assigneeIds;
    }
}