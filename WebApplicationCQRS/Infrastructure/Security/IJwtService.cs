using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Infrastructure.Security;

public interface IJwtService
{
    string GenerateJwtToken(User user, Dictionary<string, string>? customClaims = null);
    Dictionary<string, string> GetUserIdFromToken(string token);
}