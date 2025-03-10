using MediatR;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Commands;

public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ticketID = await 
                _unitOfWork.TicketRepository.AddTicket(new Ticket(request.CreatorId, request.Name, request.FileDescription,request.Description));
            return Result<int>.Success(ticketID, "Ticket Created Successfully");
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
}