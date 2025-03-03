using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Application.Features.Tickets.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationCQRS.Controllers;

[ApiController]
[Route("api/ticket")]
public class TicketsController : ControllerBase
{
    private readonly ILogger<TicketsController> _logger;
    private readonly IMediator _mediator;

    public TicketsController(ILogger<TicketsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult> CreateTicket([FromBody] CreateTicketDtoRequest command)
    {
        var userId = int.Parse(HttpContext.Items["userID"].ToString() ?? string.Empty);

        var response = await _mediator.Send(new CreateTicketCommand(userId,command.Name, command.FileDescription));
        
        return StatusCode((int)response.StatusCode, response);
        
    }

}