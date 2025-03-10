using Microsoft.EntityFrameworkCore;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Infrastructure.Persistence.Context;

public interface IApplicationContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<AssignedTicket> AssignedTickets { get; set; }
    
    public DbSet<HistoryAssignTicket> HistoryAssignTickets { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}