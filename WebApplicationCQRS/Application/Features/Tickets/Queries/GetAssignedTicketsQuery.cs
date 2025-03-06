using MediatR;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetAssignedTicketsQuery : IRequest<Result<List<DTOs.AssignedTickets>>>
{
    public GetAssignedTicketsQuery(int assignerId)
    {
        AssignerId = assignerId;
    }

    public int AssignerId { get; set; }
}