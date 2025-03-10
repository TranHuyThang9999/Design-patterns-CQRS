using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Tickets.Queries;

public class SearchTicketsHandler : IRequestHandler<SearchTicketsQuery, Result<List<AssignedTicketDetail>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchTicketsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<AssignedTicketDetail>>> Handle(SearchTicketsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var tickets = await _unitOfWork.TicketRepository.SearchTickets(request.UserId, request.TicketName);
            return Result<List<AssignedTicketDetail>>.Success(tickets);
        }
        catch (Exception e)
        {
            return Result<List<AssignedTicketDetail>>.Failure(ResponseCode.InternalError, e.Message);
        }
    }
}