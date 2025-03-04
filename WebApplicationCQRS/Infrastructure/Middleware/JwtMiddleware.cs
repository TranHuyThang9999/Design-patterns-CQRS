using Microsoft.AspNetCore.Authorization;
using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Security;

namespace WebApplicationCQRS.Infrastructure.Middleware
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public JwtMiddleware(ILogger<JwtMiddleware> logger, IJwtService jwtService, IUserRepository userRepository)
        {
            _logger = logger;
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                _logger?.LogWarning("Unauthorized request - missing or invalid token.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var token = authHeader.Substring(7);
            var tokenInfo = _jwtService.GetUserIdFromToken(token);

            if (!tokenInfo.TryGetValue("userID", out var userId) || string.IsNullOrEmpty(userId))
            {
                _logger?.LogWarning("Unauthorized request - Unable to extract userID.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var user = await _userRepository.GetUserById(int.Parse(userId));
            if (user == null)
            {
                _logger?.LogWarning("Unauthorized request - User not found.");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var lastPasswordUpdateString = tokenInfo["lastPasswordUpdate"];
            if (!DateTime.TryParse(lastPasswordUpdateString, out var lastPasswordUpdate))
            {
                _logger?.LogWarning("Unauthorized request - Invalid lastPasswordUpdate format.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            long lastPasswordChangedAtTimestamp = new DateTimeOffset(user.LastPasswordChangedAt).ToUnixTimeSeconds();
            long lastPasswordUpdateTimestamp = new DateTimeOffset(lastPasswordUpdate).ToUnixTimeSeconds();

            if (lastPasswordChangedAtTimestamp > lastPasswordUpdateTimestamp)
            {
                _logger?.LogWarning("Unauthorized request - Password changed after token issued.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(
                    $"Unauthorized {lastPasswordChangedAtTimestamp} : {lastPasswordUpdateTimestamp}");
                return;
            }


            context.Items["userID"] = userId;

            await next(context);
        }
    }
}