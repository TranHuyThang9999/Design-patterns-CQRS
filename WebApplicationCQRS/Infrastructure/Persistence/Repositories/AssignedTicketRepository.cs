using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
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

    public async Task<List<int>> CreateAssignTicketF(List<AssignedTicket> tickets, bool useTransaction = true)
    {
        _context.AssignedTickets.AddRange(tickets);

        if (!useTransaction)
        {
            await _context.SaveChangesAsync();
        }

        // ✅ Lấy danh sách ID sau khi lưu
        return tickets.Select(t => t.Id).ToList();
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