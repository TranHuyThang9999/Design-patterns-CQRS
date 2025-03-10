using Microsoft.EntityFrameworkCore;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext,IApplicationContext,IDisposable
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public AppDbContext() { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();


        modelBuilder.Entity<AssignedTicket>()
            .HasOne(at => at.Ticket)
            .WithMany()
            .HasForeignKey(at => at.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssignedTicket>()
            .HasOne(at => at.Assignee) // Người nhận
            .WithMany()
            .HasForeignKey(at => at.AssigneeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssignedTicket>()
            .HasOne(at => at.Assigner) // Người giao
            .WithMany()
            .HasForeignKey(at => at.AssignerId)
            .OnDelete(DeleteBehavior.Restrict); // Không xóa nếu người giao bị xóa
        
        modelBuilder.Entity<AssignedTicket>()
            .HasIndex(at => new { at.AssigneeId, at.TicketId ,at.Status})
            .IsUnique(); // UNIQUE Constraint
        
        modelBuilder.Entity<HistoryAssignTicket>()
            .HasOne(h => h.PreviousAssignee)
            .WithMany()
            .HasForeignKey(h => h.PreviousAssigneeId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<HistoryAssignTicket>()
            .HasOne(h => h.NewAssignee)
            .WithMany()
            .HasForeignKey(h => h.NewAssigneeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetSoftDelete();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetSoftDelete()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Deleted)
            {
                var entity = entry.Entity;
                var deletedAtProperty = entity.GetType().GetProperty("DeletedAt");

                if (deletedAtProperty != null && deletedAtProperty.PropertyType == typeof(DateTime?))
                {
                    deletedAtProperty.SetValue(entity, DateTime.UtcNow);
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Deleted)
            {
                var entity = entry.Entity;
                var deletedAtProperty = entity.GetType().GetProperty("DeletedAt");

                if (deletedAtProperty != null && deletedAtProperty.PropertyType == typeof(DateTime?))
                {
                    deletedAtProperty.SetValue(entity, DateTime.UtcNow);
                    entry.State = EntityState.Modified;
                }
            }
        }

        return base.SaveChanges();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<AssignedTicket> AssignedTickets { get; set; }
    public DbSet<HistoryAssignTicket> HistoryAssignTickets { get; set; }
}