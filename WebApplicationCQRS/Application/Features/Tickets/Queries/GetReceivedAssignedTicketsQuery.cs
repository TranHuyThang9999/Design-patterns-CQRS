using MediatR;
using WebApplicationCQRS.Application.DTOs;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetReceivedAssignedTicketsQuery : IRequest<Result<List<ReceivedAssignedTicketDTO>>>
{
    public GetReceivedAssignedTicketsQuery(int assigneeId)
    {
        AssigneeId = assigneeId;
    }

    public int AssigneeId { get; set; }
}
