using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface ITicketRepository
{
    Task<int> AddTicket(Ticket ticket);
    Task<List<Ticket>> GetTicketsByCreatorId(int creatorId);
    Task UpdateTicket(Ticket ticket);
    Task DeleteTicketsById(int[] id);
    Task<Ticket?> GetTicketById(int id);
    
    Task <bool>CheckListTicketExists(List<int> ids);
}