using MediatR;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Commands;

public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, Result<int>>
{
    private readonly ILogger<CreateTicketCommand> _logger;
    private readonly ITicketRepository _ticketRepository;
    
    public CreateTicketHandler(ILogger<CreateTicketCommand> logger, ITicketRepository ticketRepository)
    {
        _logger = logger;
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<int>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ticketID = await 
                _ticketRepository.AddTicket(new Ticket(request.CreatorId, request.Name, request.FileDescription));
            return Result<int>.Success(ticketID, "Ticket Created Successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
    
}