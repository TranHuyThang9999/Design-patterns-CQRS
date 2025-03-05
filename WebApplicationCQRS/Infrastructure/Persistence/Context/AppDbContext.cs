using Microsoft.EntityFrameworkCore;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<AssignedTicket> AssignedTickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
        
        
        modelBuilder.Entity<AssignedTicket>()
            .HasOne(at => at.Ticket)
            .WithMany()
            .HasForeignKey(at => at.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssignedTicket>()
            .HasOne(at => at.User)
            .WithMany()
            .HasForeignKey(at => at.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}