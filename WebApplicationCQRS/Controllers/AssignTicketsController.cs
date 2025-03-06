using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationCQRS.Application.Features.AssignedTickets.Commands;
using WebApplicationCQRS.Application.Resources;

namespace WebApplicationCQRS.Controllers;

[ApiController]
[Route("api/assignTickets")]
public class AssignTicketsController : ControllerBase
{
    public AssignTicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private readonly IMediator _mediator;

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult> CreateTicket([FromBody] AssignTicketsCommand command)
    {
        int? userId = HttpContextHelper.GetUserId(HttpContext);
        command.AssignerId = userId ?? 0;
        
        var response = await _mediator.Send(new  AssignTicketsCommand(command.AssignerId, command.Tickets));
        return Resources.MapResponse(this, response);
    }
}