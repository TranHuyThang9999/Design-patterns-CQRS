using NUnit.Framework;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserById(int id);
    Task<List<User>> GetActiveUsers();
    Task<int> CreateUser(User user);
    Task <bool>UpdateUserById(UserUpdateProfile user);
    Task DeleteUserById(int id);
    Task<User> GetUserByUsername(string username);
    
    Task<bool>CheckListUserExistsByUserIDs(int[]  userIDs);
}