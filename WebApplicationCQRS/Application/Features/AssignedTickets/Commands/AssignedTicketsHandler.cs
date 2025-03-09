using MediatR;
using Microsoft.EntityFrameworkCore.Migrations;
using NUnit.Framework;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class AssignedTicketsHandler : IRequestHandler<AssignTicketsCommand, Result<int>>
{
    public AssignedTicketsHandler(IAssignedTicket assignedTicket, IUserRepository userRepository,
        ITicketRepository ticketRepository, IHistoryAssignTicketRepository historyAssignTicketRepository,
        IUnitOfWork unitOfWork)
    {
        _assignedTicket = assignedTicket;
        _userRepository = userRepository;
        _ticketRepository = ticketRepository;
        _historyAssignTicketRepository = historyAssignTicketRepository;
        _unitOfWork = unitOfWork;
    }

    private readonly IAssignedTicket _assignedTicket;
    private readonly IUserRepository _userRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IHistoryAssignTicketRepository _historyAssignTicketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignedTicketsHandler()
    {
    }

    public async Task<Result<int>> Handle(AssignTicketsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                List<int> userIDs = new List<int> { request.AssignerId };
                userIDs.AddRange(request.Tickets.AssigneeIds);
                List<int> ticketIDs = new List<int>();
                ticketIDs.AddRange(request.Tickets.TicketIds);

                // ✅ 1 Kiểm tra không có ticket nào trùng nhau
                if (ticketIDs.Distinct().Count() != ticketIDs.Count)
                {
                    return Result<int>.Failure(ResponseCode.Conflict, "Duplicate tickets are not allowed.");
                }

                // ✅ 2 Kiểm tra không có 2 user trùng nhau
                if (userIDs.Distinct().Count() != userIDs.Count)
                {
                    return Result<int>.Failure(ResponseCode.Conflict, "Duplicate users are not allowed.");
                }

                var existsUserIds = await _userRepository.CheckListUserExistsByUserIDs(userIDs);
                if (!existsUserIds)
                {
                    return Result<int>.Failure(ResponseCode.NotFound, "One or more users do not exist.");
                }

                var existsTicketIds = await _ticketRepository.CheckListTicketExists(ticketIDs);
                if (!existsTicketIds)
                {
                    return Result<int>.Failure(ResponseCode.NotFound, "One or more tickets do not exist.");
                }

                List<AssignedTicket> assignedTickets = new List<AssignedTicket>();
                var historyAssignTickets = new List<HistoryAssignTicket>();

                foreach (var ticketId in ticketIDs)
                {
                    foreach (var assigneeId in request.Tickets.AssigneeIds)
                    {
                        assignedTickets.Add(new AssignedTicket
                        {
                            TicketId = ticketId,
                            AssignerId = request.AssignerId,
                            AssigneeId = assigneeId,
                            Status = AssignedTicketStatus.Assigned
                        });
                    }
                }


                var assignedTicketIds = await _assignedTicket.CreateAssignTicketF(assignedTickets, true);

                for (int i = 0; i < assignedTickets.Count; i++)
                {
                    historyAssignTickets.Add(new HistoryAssignTicket()
                    {
                        AssignedTicketId = assignedTicketIds[i],
                        PreviousAssigneeId = assignedTickets[i].AssignerId,
                        NewAssigneeId = assignedTickets[i].AssigneeId 
                    });
                }

                await _historyAssignTicketRepository.AssignTicketToAnotherUser(historyAssignTickets);

                return Result<int>.Success(assignedTickets.Count);
            });

            // ✅ Fix: Đảm bảo `result` được trả về đúng cách
            return result ?? Result<int>.Failure(ResponseCode.InternalError, "Unexpected error occurred.");
        }
        catch (Exception e)
        {
            return Result<int>.Failure(ResponseCode.InternalError, e.Message);
        }
    }
}