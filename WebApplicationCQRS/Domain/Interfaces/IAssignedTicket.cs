using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface IAssignedTicket 
{
    Task CreateAssignTicketF(List<AssignedTicket> ticket);
}