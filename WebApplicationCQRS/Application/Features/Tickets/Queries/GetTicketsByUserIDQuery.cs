using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetTicketsByUserIdQuery : IRequest<Result<List<AssignedTicketDetail>>>
{
    public int UserId { get; set; }

    public GetTicketsByUserIdQuery(int userId)
    {
        UserId = userId;
    }
}