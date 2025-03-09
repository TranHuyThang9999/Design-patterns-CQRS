using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class SearchTicketsHandler :IRequestHandler<SearchTicketsQuery, Result<List<AssignedTicketDetail>>>
{
    private readonly ITicketRepository _ticketRepository;

    public SearchTicketsHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<List<AssignedTicketDetail>>> Handle(SearchTicketsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tickets =await _ticketRepository.SearchTickets(request.UserId,request.TicketName);
            return Result<List<AssignedTicketDetail>>.Success(tickets);
        }
        catch (Exception e)
        {
            return Result<List<AssignedTicketDetail>>.Failure(ResponseCode.InternalError, e.Message);

        }

    }
}