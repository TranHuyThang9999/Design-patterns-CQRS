using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class GetTicketsByUserIDHandler : IRequestHandler<GetTicketsByUserIdQuery, Result<List<AssignedTicketDetail>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketsByUserIDHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<AssignedTicketDetail>>> Handle(GetTicketsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsByCreatorId(request.UserId);


            return Result<List<AssignedTicketDetail>>.Success(tickets);
        }
        catch (Exception e)
        {
            return Result<List<AssignedTicketDetail>>.Failure(ResponseCode.InternalError, e.Message);
        }
    }
}