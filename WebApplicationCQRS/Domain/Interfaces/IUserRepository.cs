using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserById(int id);
    Task<IEnumerable<User>> GetUsers();
    Task<int> CreateUser(User user);
    Task UpdateUserById(User user);
    Task DeleteUserById(int id);
}