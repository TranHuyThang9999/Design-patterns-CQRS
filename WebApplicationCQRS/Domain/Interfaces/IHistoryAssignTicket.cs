using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface IHistoryAssignTicketRepository
{
    Task AssignTicketToAnotherUser(List<HistoryAssignTicket> request);
}