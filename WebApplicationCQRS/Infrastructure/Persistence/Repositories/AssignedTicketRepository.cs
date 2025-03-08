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

    public async Task CreateAssignTicketF(List<AssignedTicket> ticket,bool useTransaction = true)
    {
        _context.AssignedTickets.AddRange(ticket);
        if (!useTransaction)
        {
            await _context.SaveChangesAsync();
        }
    }

    public async Task<AssignedTicket?> GetAssignedTicketById(int id)
    {
        var ticket = await _context.AssignedTickets.FindAsync(id);
        return ticket ?? null;
    }

    public async Task UpdateAssignedTicket(AssignedTicket ticket)
    {
        _context.AssignedTickets.Update(ticket);
    }

}