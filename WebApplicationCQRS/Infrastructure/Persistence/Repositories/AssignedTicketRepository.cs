using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Persistence.Context;

namespace WebApplicationCQRS.Infrastructure.Persistence.Repositories;

public class AssignedTicketRepository : IAssignedTicket
{
    private readonly AppDbContext _context;

    public AssignedTicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task CreateAssignTicketF(List<AssignedTicket> ticket)
    {
        _context.AssignedTickets.AddRange(ticket);
        return _context.SaveChangesAsync();
    }
}