using MediatR;
using WebApplicationCQRS.Application.DTOs;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class SearchTicketsQuery : IRequest<Result<List<AssignedTicketDetail>>>
{
    public SearchTicketsQuery(int userId, string ticketName)
    {
        UserId = userId;
        TicketName = ticketName;
    }

    public int UserId { get; set; }
    public string TicketName { get; set; }
}