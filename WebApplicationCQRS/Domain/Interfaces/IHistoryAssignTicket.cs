using WebApplicationCQRS.Application.Features.AssignedTickets.Commands;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface IHistoryAssignTicketRepository
{
    Task<int> AssignTicketToAnotherUser(HistoryAssignTicket request);
}