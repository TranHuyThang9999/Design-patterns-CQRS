using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Infrastructure.Security
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateJwtToken(User user, Dictionary<string, string>? customClaims = null)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User cannot be null when generating JWT.");
                }

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name)
                };

                if (customClaims != null)
                {
                    foreach (var customClaim in customClaims)
                    {
                        claims.Add(new Claim(customClaim.Key, customClaim.Value));
                    }
                }

                var secretKey = _configuration["Jwt:SecretKey"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new InvalidOperationException("JWT SecretKey is missing in configuration.");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expirationConfig = _configuration["Jwt:Expiration"];
                if (string.IsNullOrEmpty(expirationConfig) || !double.TryParse(expirationConfig, out double expiration))
                {
                    throw new InvalidOperationException("Invalid or missing JWT Expiration value.");
                }

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expiration),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred while generating JWT token: {Message}", ex.Message);
                throw;
            }
        }


        public Dictionary<string, string> GetUserIdFromToken(string token)
        {
            var result = new Dictionary<string, string>();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    _logger?.LogWarning("Token parsing failed.");
                    return result;
                }

                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userID")?.Value;
                var lastPasswordUpdate = jwtToken.Claims.FirstOrDefault(c => c.Type == "lastPasswordUpdate")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    result["userID"] = userId;
                }
                else
                {
                    _logger?.LogWarning("Token is missing 'userID' claim.");
                }

                if (!string.IsNullOrEmpty(lastPasswordUpdate))
                {
                    result["lastPasswordUpdate"] = lastPasswordUpdate;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning("Token validation failed: {Message}", ex.Message);
                return new Dictionary<string, string>();
            }
        }
    }
}