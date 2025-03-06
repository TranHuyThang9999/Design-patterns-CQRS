using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface IAssignedTicket
{
    /// <summary>
    /// Tạo danh sách ticket được assign.
    /// </summary>
    Task CreateAssignTicketF(List<AssignedTicket> tickets);
}