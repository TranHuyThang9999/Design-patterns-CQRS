using MediatR;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetAssignedTicketsHandler : IRequestHandler<GetAssignedTicketsQuery, Result<List<DTOs.AssignedTickets>>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetAssignedTicketsHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<List<DTOs.AssignedTickets>>> Handle(GetAssignedTicketsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tickets = await _ticketRepository.GetTicketsAssignedByMe(request.AssignerId);
            return Result<List<DTOs.AssignedTickets>>.Success(tickets);
        }
        catch (Exception ex)
        {
            return Result<List<DTOs.AssignedTickets>>.Failure(ResponseCode.InternalError,ex.Message);
        }
    }
}