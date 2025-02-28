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

}