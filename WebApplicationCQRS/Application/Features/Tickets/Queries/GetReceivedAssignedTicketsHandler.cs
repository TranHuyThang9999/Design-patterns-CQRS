using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetReceivedAssignedTicketsHandler : IRequestHandler<GetReceivedAssignedTicketsQuery, Result<List<ReceivedAssignedTicketDTO>>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetReceivedAssignedTicketsHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<List<ReceivedAssignedTicketDTO>>> Handle(GetReceivedAssignedTicketsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tickets = await _ticketRepository.GetTicketsAssignedToMe(request.AssigneeId);
            return Result<List<ReceivedAssignedTicketDTO>>.Success(tickets);
        }
        catch (Exception e)
        {
           return Result<List<ReceivedAssignedTicketDTO>>.Failure(ResponseCode.InternalError,e.Message);
        }
    }
}