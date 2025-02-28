using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplicationCQRS.Application.Features.Users.Commands;

namespace WebApplicationCQRS.Controllers;

[ApiController]
[Route("api/user")]
public class UsersController:ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IMediator _mediator;
    public UsersController(ILogger<UsersController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { UserId = userId });
    }
}