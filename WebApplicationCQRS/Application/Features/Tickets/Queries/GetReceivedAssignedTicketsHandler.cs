using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetReceivedAssignedTicketsHandler : IRequestHandler<GetReceivedAssignedTicketsQuery, Result<List<ReceivedAssignedTicketDTO>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetReceivedAssignedTicketsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ReceivedAssignedTicketDTO>>> Handle(GetReceivedAssignedTicketsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsAssignedToMe(request.AssigneeId);
            return Result<List<ReceivedAssignedTicketDTO>>.Success(tickets);
        }
        catch (Exception e)
        {
           return Result<List<ReceivedAssignedTicketDTO>>.Failure(ResponseCode.InternalError,e.Message);
        }
    }
}