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
        try
        {
            var result = await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                List<int> userIDs = new List<int> { request.PreviousAssigneeId };
                userIDs.AddRange(request.NewAssigneeIds);
                List<int> ticketIDs = new List<int>();
                ticketIDs.AddRange(request.AssignedTicketIds);

                // ✅ 2️⃣ Kiểm tra không có ticket nào trùng nhau
                if (ticketIDs.Distinct().Count() != ticketIDs.Count)
                {
                    return Result<int>.Failure(ResponseCode.Conflict, "Duplicate tickets are not allowed.");
                }

                // ✅ 3️⃣ Kiểm tra không có 2 user trùng nhau
                if (userIDs.Distinct().Count() != userIDs.Count)
                {
                    return Result<int>.Failure(ResponseCode.Conflict, "Duplicate users are not allowed.");
                }

                var historyAssignTickets = new List<HistoryAssignTicket>
                {
                    new HistoryAssignTicket(
                        
                        // AssignedTicketId = request.AssignedTicketId,
                        // OldAssigneeId = request.OldAssigneeId,
                        // NewAssigneeId = request.NewAssigneeId
                    )
                };
                
                    await _historyAssignTicketRepository.AssignTicketToAnotherUser(historyAssignTickets);

                var assaign = await _assignedTicket.GetAssignedTicketById(0);
                if (assaign is null)
                {
                    return Result<int>.Failure(ResponseCode.NotFound, "Ticket not found");
                }

                var assignedTickets = new List<AssignedTicket>
                {
                    new AssignedTicket
                    {
                        TicketId = assaign.TicketId,
                        AssignerId = request.PreviousAssigneeId,
                        AssigneeId = request.NewAssigneeIds[0],
                        Status = AssignedTicketStatus.Reassigned
                    }
                };

                await _assignedTicket.CreateAssignTicketF(assignedTickets);

                assaign.Status = AssignedTicketStatus.Transferred;
                await _assignedTicket.UpdateAssignedTicket(assaign);
                return Result<int>.Success(0);
            }, cancellationToken);

            // 🔹 Nếu `result` là null, trả về lỗi chung chung
            return result ?? Result<int>.Failure(ResponseCode.InternalError, "Unexpected error occurred.");
        }
        catch (Exception)
        {
            return Result<int>.Failure(ResponseCode.InternalError, "error in reassigning ticket");
        }
    }
}