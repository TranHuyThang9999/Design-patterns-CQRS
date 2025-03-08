using Microsoft.EntityFrameworkCore;
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

    public async Task CreateAssignTicketF(List<AssignedTicket> ticket, bool useTransaction = true)
    {
        _context.AssignedTickets.AddRange(ticket);
        if (!useTransaction)
        {
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<AssignedTicket>> GetAssignedTicketsByIds(List<int> ids)
    {
        return await _context.AssignedTickets.Where(t => ids.Contains(t.Id)).ToListAsync();
    }


    public async Task UpdateAssignedTickets(List<AssignedTicket> tickets)
    {
        _context.AssignedTickets.UpdateRange(tickets);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AreAllAssignedTicketsExist(List<int> assignTicketIds)
    {
        if (assignTicketIds == null || assignTicketIds.Count == 0)
        {
            return false;
        }

        int existingCount = await _context.AssignedTickets
            .Where(t => assignTicketIds.Contains(t.Id))
            .CountAsync();

        return existingCount == assignTicketIds.Count;
    }


}