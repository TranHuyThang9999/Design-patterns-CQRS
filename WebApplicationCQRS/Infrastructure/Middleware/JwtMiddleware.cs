using Microsoft.AspNetCore.Authorization;

using WebApplicationCQRS.Infrastructure.Security;

namespace WebApplicationCQRS.Infrastructure.Middleware
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly IJwtService _jwtService;

        public JwtMiddleware(ILogger<JwtMiddleware> logger, IJwtService jwtService)
        {
            _logger = logger;
            _jwtService = jwtService;
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

            await next(context);
        }
    }
}
