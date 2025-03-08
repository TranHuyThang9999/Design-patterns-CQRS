using System.Text.Json.Serialization;
using MediatR;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class ReassignTicketCommand : IRequest<Result<int>>
{
    public ReassignTicketCommand(List<int> assignedTicketIds, int previousAssigneeId, List<int> newAssigneeIds)
    {
        AssignedTicketIds = assignedTicketIds;
        PreviousAssigneeId = previousAssigneeId;
        NewAssigneeIds = newAssigneeIds;
    }
    
    public ReassignTicketCommand()
    {
    }

    /// Danh sách ID của các ticket cần được reassigned.
    public List<int> AssignedTicketIds { get; set; }

    /// ID của người được assign trước đó (người cũ). for me
    [JsonIgnore]
    public int PreviousAssigneeId { get; set; }

    /// Danh sách ID của những người mới sẽ nhận ticket.
    public List<int> NewAssigneeIds { get; set; }
}