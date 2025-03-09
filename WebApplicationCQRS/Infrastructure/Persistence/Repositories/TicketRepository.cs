using Microsoft.EntityFrameworkCore;
using WebApplicationCQRS.Application.DTOs;
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

    /// Lấy danh sách ticket mà người dùng hiện tại được assign.
    public async Task<List<ReceivedAssignedTicketDTO>> GetTicketsAssignedToMe(int userId)
    {
        return await _context.AssignedTickets
            .Where(at => at.AssigneeId == userId)
            .Join(
                _context.Tickets,
                at => at.TicketId,
                t => t.Id,
                (at, t) => new { at, t }
            )
            .Join(
                _context.Users, 
                at_t => at_t.at.AssignerId, 
                u => u.Id, 
                (at_t, u) => new ReceivedAssignedTicketDTO
                {
                    Id =  at_t.at.Id,
                    AssignedTicketId = at_t.at.Id,
                    TicketId = at_t.t.Id,
                    Name = at_t.t.Name,
                    Description = at_t.t.Description,
                    FileDescription = at_t.t.FileDescription,
                    AssignerId = at_t.at.AssignerId,
                    NameUserAssignerIdTicket = u.Name,
                    TimeAssign = at_t.at.UpdatedAt ??  DateTime.UtcNow,
                }
            )
            .ToListAsync();
    }



    /// Lấy danh sách ticket mà đã assign cho người khác.
    public async Task<List<AssignedTickets>> GetTicketsAssignedByMe(int userId)
    {
        return await _context.AssignedTickets
            .Where(at => at.AssignerId == userId)
            .Join(
                _context.Tickets,
                at => at.TicketId,
                t => t.Id,
                (at, t) => new AssignedTickets
                {
                    Id = t.Id,
                    AssigneeId = at.AssigneeId,
                    Name = t.Name,
                    FileDescription = t.FileDescription,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt,
                }
            )
            .ToListAsync();
    }

    public async Task<bool> CheckIfUserIsCreatorOfTickets(int creatorId, List<int> ticketIds)
    {
        var count = await _context.Tickets
            .Where(t => t.CreatorId == creatorId && ticketIds.Contains(t.Id))
            .CountAsync();

        return count == ticketIds.Count;
    }
}