using MediatR;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class ReassignTicketHandler : IRequestHandler<ReassignTicketCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReassignTicketHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(ReassignTicketCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                var assignedTickets = new List<AssignedTicket>();
                var historyAssignTickets = new List<HistoryAssignTicket>();

                List<int> userIDs = new List<int> { request.PreviousAssigneeId };
                userIDs.AddRange(request.NewAssigneeIds);
                List<int> ticketIDs = new List<int>();
                ticketIDs.AddRange(request.AssignedTicketIds);

                // ✅ 1⃣ Kiểm tra không có ticket nào trùng nhau
                if (ticketIDs.Distinct().Count() != ticketIDs.Count)
                {
                    return Result<int>.Failure(ResponseCode.Conflict, "Duplicate tickets are not allowed.");
                }

                // ✅ 2 Kiểm tra không có 2 user trùng nhau
                if (userIDs.Distinct().Count() != userIDs.Count)
                {
                    return Result<int>.Failure(ResponseCode.Conflict, "Duplicate users are not allowed.");
                }

                //
                var assignedTicketsExist =
                    await _unitOfWork.AssignedTicket.AreAllAssignedTicketsExist(request.AssignedTicketIds);
                if (!assignedTicketsExist)
                {
                    return Result<int>.Failure(ResponseCode.NotFound, "No assigned tickets exist.");
                }

                var pendingAssignedTicketsUpdates = await _unitOfWork.AssignedTicket.GetAssignedTicketsByIds(ticketIDs);
                if (pendingAssignedTicketsUpdates is null)
                {
                    return Result<int>.Failure(ResponseCode.NotFound, "Ticket not found");
                }

                for (int i = 0; i < pendingAssignedTicketsUpdates.Count; i++)
                {
                    for (int j = 0; j < request.NewAssigneeIds.Count; j++)
                    {
                        historyAssignTickets.Add(new HistoryAssignTicket()
                        {
                            PreviousAssigneeId = request.PreviousAssigneeId,
                            NewAssigneeId = request.NewAssigneeIds[j],
                            AssignedTicketId = pendingAssignedTicketsUpdates[i].Id
                        });

                        assignedTickets.Add(new AssignedTicket()
                        {
                            AssigneeId = request.NewAssigneeIds[j],
                            AssignerId = request.PreviousAssigneeId,
                            TicketId = pendingAssignedTicketsUpdates[i].TicketId,
                        });
                    }
                }

                await _unitOfWork.AssignedTicket.CreateAssignTicketF(assignedTickets);

                await _unitOfWork.HistoryAssignTicketRepository.AssignTicketToAnotherUser(historyAssignTickets);

                if (pendingAssignedTicketsUpdates?.Any() ?? false)
                {
                    foreach (var ticket in pendingAssignedTicketsUpdates)
                    {
                        ticket.Status = AssignedTicketStatus.Reassigned;
                    }

                    await _unitOfWork.AssignedTicket.UpdateAssignedTickets(pendingAssignedTicketsUpdates);
                }

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