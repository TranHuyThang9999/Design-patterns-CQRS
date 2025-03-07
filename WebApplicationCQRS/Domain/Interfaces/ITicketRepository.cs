using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface ITicketRepository
{
    Task<int> AddTicket(Ticket ticket);
    Task<List<Ticket>> GetTicketsByCreatorId(int creatorId);
    Task UpdateTicket(Ticket ticket);
    Task DeleteTicketsById(int[] id);
    Task<Ticket?> GetTicketById(int id);

    Task<bool> CheckListTicketExists(List<int> ids);


    /// Lấy danh sách ticket mà người dùng hiện tại được assign.
    Task<List<ReceivedAssignedTicketDTO>> GetTicketsAssignedToMe(int userId);

    /// Lấy danh sách ticket mà người dùng hiện tại đã assign cho người khác.
    Task<List<AssignedTickets>> GetTicketsAssignedByMe(int userId);

    Task<bool> CheckIfUserIsCreatorOfTickets(int creatorId, List<int> ticketIds);
}