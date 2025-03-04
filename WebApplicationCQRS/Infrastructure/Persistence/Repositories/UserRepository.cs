using Microsoft.EntityFrameworkCore;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Persistence.Context;

namespace WebApplicationCQRS.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
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


    public async Task<bool> UpdateUserById(UserUpdateProfile userDto)
    {
        int rowsAffected = await _context.Users
            .Where(u => u.Id == userDto.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.AvatarUrl, userDto.AvatarUrl)
                .SetProperty(u => u.Email, userDto.Email)
            );

        return rowsAffected > 0;
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