using Microsoft.EntityFrameworkCore;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Persistence.Context;

namespace WebApplicationCQRS.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserById(int id) =>
        await _context.Users.FindAsync(id);

    public async Task<IEnumerable<User>> GetUsers() =>
        await _context.Users.ToListAsync();

    public async Task<int> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task UpdateUserById(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
    }
}