using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApplicationCQRS.Infrastructure.Security
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string? GenerateToken(int userId)
        {
            try
            {
                var keyString = _config["Jwt:SecretKey"];
                if (string.IsNullOrEmpty(keyString))
                {
                    throw new ArgumentNullException(nameof(keyString), "JWT Key is missing in configuration.");
                }

                var key = Encoding.UTF8.GetBytes(keyString);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"[JWT Error] Missing key: {ex.Message}");
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"[JWT Error] Security token issue: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[JWT Error] Unexpected error: {ex.Message}");
            }

            return null;
        }


        public int? ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(userId, out var id) ? id : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
