using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Security;

namespace WebApplicationCQRS.Infrastructure.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtService jwtService, IUserRepository userRepository)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtService.ValidateToken(token);

            if (userId != null)
            {
                context.Items["User"] = await userRepository.GetUserById(userId.Value);
            }

            await _next(context);
        }
    }
}