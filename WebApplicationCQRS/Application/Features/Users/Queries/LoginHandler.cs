using System.Globalization;
using System.Net;
using MediatR;
using WebApplicationCQRS.Domain;
using WebApplicationCQRS.Domain.Interfaces;
using WebApplicationCQRS.Infrastructure.Security;

namespace WebApplicationCQRS.Application.Features.Users.Queries;

public class LoginHandler : IRequestHandler<LoginQuery, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<LoginHandler> _logger;
    private readonly IJwtService _jwtService;

    public LoginHandler(IUserRepository userRepository, ILogger<LoginHandler> logger, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _jwtService = jwtService;
    }

    public async Task<Result<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetUserByUsername(request.UserName);
            if (user is null)
            {
                return Result<string>.Failure(ResponseCode.Conflict, "User not found", HttpStatusCode.NotFound);
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Result<string>.Failure(ResponseCode.Conflict, "User not found", HttpStatusCode.NotFound);
            }

            var customClaims = new Dictionary<string, string>
            {
                { "userID", user.Id.ToString() },
                { "lastPasswordUpdate", user.Updated.ToString(CultureInfo.InvariantCulture) }
            };

            var token = _jwtService.GenerateJwtToken(user, customClaims);
            return Result<string>.Success(token);

        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result<string>.Failure(ResponseCode.InternalError, "Internal Server Error", HttpStatusCode.InternalServerError);
        }
    }
}