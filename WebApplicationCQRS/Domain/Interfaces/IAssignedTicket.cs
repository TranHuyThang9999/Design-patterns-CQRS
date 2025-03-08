using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface IAssignedTicket
{
    /// Tạo danh sách ticket được assign.
    Task CreateAssignTicketF(List<AssignedTicket> tickets,bool useTransaction = true);
    Task<AssignedTicket>GetAssignedTicketById(int id);
    Task UpdateAssignedTicket(AssignedTicket ticket);
}