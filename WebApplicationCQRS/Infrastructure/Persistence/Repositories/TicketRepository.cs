using Microsoft.EntityFrameworkCore;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Persistence.Context;

namespace WebApplicationCQRS.Infrastructure.Persistence.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddTicket(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket.Id;
    }

    public async Task<List<Ticket>> GetTicketsByCreatorId(int creatorId)
    {
        return await _context.Tickets.Where(u => u.CreatorId == creatorId).ToListAsync();
    }

    public async Task UpdateTicket(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }


    public async Task DeleteTicketsById(int[] ids)
    {
        await _context.Tickets.Where(t => ids.Contains(t.Id)).ExecuteDeleteAsync();
    }

    public async Task<Ticket?> GetTicketById(int id)
    {
        return await _context.Tickets.Where(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckListTicketExists(List<int> ids)
    {
        if (ids == null || ids.Count == 0)
            return false;

        var count = await _context.Tickets
            .Where(u => ids.Contains(u.Id))
            .CountAsync();

        return count == ids.Count;
    }


    
}