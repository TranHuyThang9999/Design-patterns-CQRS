using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Application.Features.Users.Commands;
using WebApplicationCQRS.Application.Features.Users.Queries;

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
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("profile")]
    public async Task<ActionResult<List<UserDto>>> Profile()
    {
        var response = await _mediator.Send(new GetUserQuery(5));
        return Ok(response);
    }
}