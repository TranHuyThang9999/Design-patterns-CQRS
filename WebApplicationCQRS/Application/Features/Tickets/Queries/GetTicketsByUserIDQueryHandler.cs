using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetTicketsByUserIdQueryHandler : IRequestHandler<GetTicketsByUserIdQuery, Result<List<AssignedTicketDetail>>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketsByUserIdQueryHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<List<AssignedTicketDetail>>> Handle(GetTicketsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
         //   List<TicketDtoResponse> ticketMapper = new List<TicketDtoResponse>();

            var tickets = await _ticketRepository.GetTicketsByCreatorId(request.UserId);
          

            return Result<List<AssignedTicketDetail>>.Success(tickets);
        }
        catch (Exception e)
        {
            return Result<List<AssignedTicketDetail>>.Failure(ResponseCode.InternalError, e.Message);
        }
    }
}