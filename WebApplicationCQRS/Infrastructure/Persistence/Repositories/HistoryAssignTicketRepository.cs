using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Persistence.Context;

namespace WebApplicationCQRS.Infrastructure.Persistence.Repositories;

public class HistoryAssignTicketRepository :IHistoryAssignTicketRepository
{
    private readonly AppDbContext _context;

    public HistoryAssignTicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AssignTicketToAnotherUser(HistoryAssignTicket request)
    {
        await _context.HistoryAssignTickets.AddAsync(request);
        await _context.SaveChangesAsync();
        return request.Id;
    }
}