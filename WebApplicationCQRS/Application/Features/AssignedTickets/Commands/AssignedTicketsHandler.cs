using MediatR;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class AssignedTicketsHandler : IRequestHandler<AssignTicketsCommand, Result<int>>
{
    private readonly IAssignedTicket _assignedTicket;

    public AssignedTicketsHandler(IAssignedTicket assignedTicket)
    {
        _assignedTicket = assignedTicket;
    }

    public async Task<Result<int>> Handle(AssignTicketsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var assignedTickets = request.Tickets.Select(ticket => new AssignedTicket
            {
                TicketId = ticket.TicketId,
                AssigneeId = ticket.AssigneeId,
                AssignerId = ticket.AssignerId
            }).ToList();

            await _assignedTicket.CreateAssignTicketF(assignedTickets);

            return Result<int>.Success(assignedTickets.Count);
        }
        catch (Exception e)
        {
            return Result<int>.Failure(ResponseCode.InternalError, e.Message);
        }
    }

    
}