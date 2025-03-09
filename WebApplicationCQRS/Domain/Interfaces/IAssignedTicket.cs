using NUnit.Framework;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface IAssignedTicket
{
    /// Tạo danh sách ticket được assign.
    Task <List<int>>CreateAssignTicketF(List<AssignedTicket> tickets, bool useTransaction = true);

    Task<List<AssignedTicket>> GetAssignedTicketsByIds(List<int> ids);
    Task UpdateAssignedTickets(List<AssignedTicket> tickets);
    Task<bool> AreAllAssignedTicketsExist(List<int> assignTicketIds);
}