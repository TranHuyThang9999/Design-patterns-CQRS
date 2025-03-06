using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetTicketsByUserIdQueryHandler :IRequestHandler<GetTicketsByUserIDQuery, Result<List<TicketDtoResponse>>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketsByUserIdQueryHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<List<TicketDtoResponse>>> Handle(GetTicketsByUserIDQuery request, CancellationToken cancellationToken)
    {
        try
        {   List<TicketDtoResponse> ticketMapper = new List<TicketDtoResponse>();
            
            var tickets = await _ticketRepository.GetTicketsByCreatorId(request.UserId);
            foreach (var VARIABLE in tickets)
            {
                ticketMapper.Add(new TicketDtoResponse()
                {
                    Id = VARIABLE.Id, 
                    Name = VARIABLE.Name,
                    FileDescription =  VARIABLE.FileDescription,
                    Description = VARIABLE.Description,
                });
            }
            return Result<List<TicketDtoResponse>>.Success(ticketMapper);
        }
        catch (Exception e)
        {
            return Result<List<TicketDtoResponse>>.Failure(ResponseCode.InternalError, e.Message);
        }
    }
    
}