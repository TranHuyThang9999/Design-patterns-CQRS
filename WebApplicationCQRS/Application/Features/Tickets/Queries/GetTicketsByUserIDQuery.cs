using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetTicketsByUserIDQuery : IRequest<Result<List<TicketDtoResponse>>>
{
    public int UserId { get; set; }

    public GetTicketsByUserIDQuery(int userId)
    {
        UserId = userId;
    }
}