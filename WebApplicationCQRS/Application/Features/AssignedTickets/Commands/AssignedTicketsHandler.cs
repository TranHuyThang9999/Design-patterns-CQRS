using MediatR;
using NUnit.Framework;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class AssignedTicketsHandler : IRequestHandler<AssignTicketsCommand, Result<int>>
{
    private readonly IAssignedTicket _assignedTicket;
    private readonly IUserRepository _userRepository;
    private readonly ITicketRepository _ticketRepository;

    public AssignedTicketsHandler()
    {
    }

    public AssignedTicketsHandler(IAssignedTicket assignedTicket, IUserRepository userRepository,
        ITicketRepository ticketRepository)
    {
        _assignedTicket = assignedTicket;
        _userRepository = userRepository;
        _ticketRepository = ticketRepository;
    }


    public async Task<Result<int>> Handle(AssignTicketsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<int> userIDs = new List<int> { request.AssignerId };
            userIDs.AddRange(request.Tickets.AssigneeIds);
            List<int> ticketIDs = new List<int>();
            ticketIDs.AddRange(request.Tickets.TicketIds);

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

            // ✅ 4️⃣ Kiểm tra AssignerId không trùng với AssigneeIds
            if (request.Tickets.AssigneeIds.Contains(request.AssignerId))
            {
                return Result<int>.Failure(ResponseCode.Conflict, "Assigner cannot be an assignee.");
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

            foreach (var ticketId in ticketIDs)
            {
                foreach (var assigneeId in request.Tickets.AssigneeIds)
                {
                    assignedTickets.Add(new AssignedTicket
                    {
                        TicketId = ticketId,
                        AssignerId = request.AssignerId,
                        AssigneeId = assigneeId
                    });
                }
            }
            await _assignedTicket.CreateAssignTicketF(assignedTickets);

            return Result<int>.Success(assignedTickets.Count);
        }
        catch (Exception e)
        {
            return Result<int>.Failure(ResponseCode.InternalError, e.Message);
        }
    }
}