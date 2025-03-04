using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Application.Features.Users.Commands;
using WebApplicationCQRS.Application.Features.Users.Queries;

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
        return StatusCode((int)response.StatusCode, new
        {
            response.Code,
            response.Message,
        });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<List<UserDto>>> Profile()
    {
        var userId = int.Parse(HttpContext.Items["userID"].ToString() ?? string.Empty);
        var response = await _mediator.Send(new GetUserQuery(userId));
        return StatusCode((int)response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginQuery loginDto)
    {
        var response = await _mediator.Send(loginDto);

        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize]
    [HttpPatch("profile/update")]
    public async Task<ActionResult> UpdateProfile([FromBody] UpdateUserRequest command)
    {
        var userId = int.Parse(HttpContext.Items["userID"].ToString() ?? string.Empty);

        var response = await _mediator.Send(new UpdateProfileCommand(userId, command.Email, command.AvatarUrl));

        return StatusCode((int)response.StatusCode, response);
    }

    [Authorize]
    [HttpPatch("profile/changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest command)
    {
        var userId = int.Parse(HttpContext.Items["userID"].ToString() ?? string.Empty);
        var response = await _mediator.Send(new ChangePasswordCommand(userId, command.CurrentPassword, command.NewPassword));
        
        return StatusCode((int)response.StatusCode, response);
    }
}