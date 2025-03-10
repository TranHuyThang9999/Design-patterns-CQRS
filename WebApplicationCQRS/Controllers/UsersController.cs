using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Application.Features.Users.Commands;
using WebApplicationCQRS.Application.Features.Users.Queries;
using WebApplicationCQRS.Application.Resources;

namespace WebApplicationCQRS.Controllers;

[ApiController]
[Route("api/user")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IMediator _mediator;

    public UsersController(ILogger<UsersController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var response = await _mediator.Send(command);
        return Resources.MapResponse(this,response);

    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<List<UserDto>>> Profile()
    {
        int? userId = HttpContextHelper.GetUserId(HttpContext);
        var response = await _mediator.Send(new GetUserQuery(userId ?? 0));
        return Resources.MapResponse(this,response);

    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginQuery loginDto)
    {
        var response = await _mediator.Send(loginDto);

        return Resources.MapResponse(this,response);

    }

    [Authorize]
    [HttpPatch("profile/update")]
    public async Task<ActionResult> UpdateProfile([FromBody] UpdateUserRequest command)
    {
        var userId = int.Parse(HttpContext.Items["userID"]?.ToString() ?? string.Empty);

        var response = await _mediator.Send(new UpdateProfileCommand(userId, command.Email, command.AvatarUrl));

        return Resources.MapResponse(this,response);
    }

    [Authorize]
    [HttpPatch("profile/changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest command)
    {
        int? userId = HttpContextHelper.GetUserId(HttpContext);
        var response = await _mediator.Send(new ChangePasswordCommand(userId ?? 0, command.CurrentPassword, command.NewPassword));
        
        return Resources.MapResponse(this,response);
    }

    [Authorize]
    [HttpGet("public/users")]
    public async Task<ActionResult<List<UserDto>>> PublicUser()
    {
        int? userId = HttpContextHelper.GetUserId(HttpContext);
        var response = await _mediator.Send(new GetUsersQuery(userId ?? 0));
        return Resources.MapResponse(this,response);
    }
    
}