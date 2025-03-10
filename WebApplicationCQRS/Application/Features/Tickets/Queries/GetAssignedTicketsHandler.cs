using MediatR;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetAssignedTicketsHandler : IRequestHandler<GetAssignedTicketsQuery, Result<List<DTOs.AssignedTickets>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAssignedTicketsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<DTOs.AssignedTickets>>> Handle(GetAssignedTicketsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsAssignedByMe(request.AssignerId);
            return Result<List<DTOs.AssignedTickets>>.Success(tickets);
        }
        catch (Exception ex)
        {
            return Result<List<DTOs.AssignedTickets>>.Failure(ResponseCode.InternalError,ex.Message);
        }
    }
}