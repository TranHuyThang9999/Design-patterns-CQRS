using MediatR;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class ReassignTicketHandler : IRequestHandler<ReassignTicketCommand, Result<int>>
{
    private readonly IHistoryAssignTicketRepository _historyAssignTicketRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IAssignedTicket _assignedTicket;
    private readonly IUnitOfWork _unitOfWork;

    public ReassignTicketHandler(IHistoryAssignTicketRepository historyAssignTicketRepository,
        ITicketRepository ticketRepository, IAssignedTicket assignedTicket, IUnitOfWork unitOfWork)
    {
        _historyAssignTicketRepository = historyAssignTicketRepository;
        _ticketRepository = ticketRepository;
        _assignedTicket = assignedTicket;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(ReassignTicketCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            try
            {
                if (request.NewAssigneeId == request.OldAssigneeId)
                {
                    return Result<int>.Failure(ResponseCode.Conflict, "Duplicate tickets are not allowed.");
                }

                var model = new HistoryAssignTicket
                {
                    AssignedTicketId = request.AssignedTicketId,
                    OldAssigneeId = request.OldAssigneeId,
                    NewAssigneeId = request.NewAssigneeId
                };

                var historyAssignTicketId = await _historyAssignTicketRepository.AssignTicketToAnotherUser(model);

                var assaign = await _assignedTicket.GetAssignedTicketById(request.AssignedTicketId);
                if (assaign is null)
                {
                    return Result<int>.Failure(ResponseCode.NotFound, "Ticket not found");
                }

                var assignedTickets = new List<AssignedTicket>
                {
                    new AssignedTicket
                    {
                        TicketId = assaign.TicketId,
                        AssignerId = request.OldAssigneeId,
                        AssigneeId = assaign.AssigneeId,
                        Status = AssignedTicketStatus.Reassigned
                    }
                };

                await _assignedTicket.CreateAssignTicketF(assignedTickets);
                
                assaign.Status = AssignedTicketStatus.Transferred;
                await _assignedTicket.UpdateAssignedTicket(assaign);
                return Result<int>.Success(historyAssignTicketId);
            }
            catch (Exception e)
            {
                return Result<int>.Failure(ResponseCode.InternalError, e.Message);
            }
        }, cancellationToken);
    }
}