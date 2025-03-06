using MediatR;
using Microsoft.AspNetCore.Authorization;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Application.Features.Tickets.Commands;
using Microsoft.AspNetCore.Mvc;
using WebApplicationCQRS.Application.Features.Tickets.Queries;
using WebApplicationCQRS.Application.Resources;

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
        int? userId = HttpContextHelper.GetUserId(HttpContext);

        var response = await _mediator.Send(new CreateTicketCommand(userId ?? 0,command.Name, command.FileDescription,command.Description));
        
        return Resources.MapResponse(this,response);
    }

    [Authorize]
    [HttpGet("tickets")]
    public async Task<ActionResult> GetTicketsByUserId()
    {
        int? userId = HttpContextHelper.GetUserId(HttpContext);
        var response = await _mediator.Send(new GetTicketsByUserIdQuery(userId ?? 0));
        
        return Resources.MapResponse(this,response);
    }

    [Authorize]
    [HttpGet("ticketsAssignedByMe")]
    public async Task<ActionResult> GetTicketsAssignedByMe()
    {
        int? userId = HttpContextHelper.GetUserId(HttpContext);
        var response = await _mediator.Send(new GetAssignedTicketsQuery(userId ?? 0));
        
        return Resources.MapResponse(this,response);
    }
    
}