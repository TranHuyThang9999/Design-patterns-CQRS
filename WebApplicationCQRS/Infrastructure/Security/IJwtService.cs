namespace WebApplicationCQRS.Infrastructure.Security;

public interface IJwtService
{
    string GenerateToken(int userId);
    int? ValidateToken(string token);
}