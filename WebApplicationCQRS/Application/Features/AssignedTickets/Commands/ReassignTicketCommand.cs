using System.Text.Json.Serialization;
using MediatR;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class ReassignTicketCommand : IRequest<Result<int>>
{
    public ReassignTicketCommand(int assignedTicketId, int oldAssigneeId, int newAssigneeId)
    {
        AssignedTicketId = assignedTicketId;
        OldAssigneeId = oldAssigneeId;
        NewAssigneeId = newAssigneeId;
    }

    public ReassignTicketCommand()
    {
    }
    public int AssignedTicketId { get; set; }

    /// ID của người được assign trước đó (người cũ).
    public int OldAssigneeId { get; set; }

    /// ID của người mới được assign ticket.
    public int NewAssigneeId { get; set; }
}