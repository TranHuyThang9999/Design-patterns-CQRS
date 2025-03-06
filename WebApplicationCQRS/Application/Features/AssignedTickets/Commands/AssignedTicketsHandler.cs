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

    public AssignedTicketsHandler(IAssignedTicket assignedTicket, IUserRepository userRepository, ITicketRepository ticketRepository)
    {
        _assignedTicket = assignedTicket;
        _userRepository = userRepository;
        _ticketRepository = ticketRepository;
    }


    public async Task<Result<int>> Handle(AssignTicketsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<int>userIDs = new List<int>();
            List<int> ticketIDs = new List<int>();
            
            // 1️⃣ Kiểm tra: Một người nhận nhiều ticket, nhưng không có ticket nào trùng nhau
            var hasDuplicateTickets = request.Tickets
                .GroupBy(t => new { t.AssigneeId, t.TicketId })
                .Any(g => g.Count() > 1);

            if (hasDuplicateTickets)
            {
                return Result<int>.Failure(ResponseCode.Conflict, "A user cannot be assigned the same ticket multiple times.");            }

            // 2️⃣ Kiểm tra: Không có 2 user giống nhau trong danh sách (mỗi user chỉ xuất hiện một lần)
            var hasDuplicateAssignee = request.Tickets
                .GroupBy(t => t.AssigneeId)
                .Any(g => g.Count() > 1);

            if (hasDuplicateAssignee)
            {
                return Result<int>.Failure(ResponseCode.Conflict, "The assigner and the assignee cannot be the same.");            }

            // 3️⃣ Kiểm tra: AssignerId và AssigneeId không được trùng nhau
            var hasSelfAssignedTicket = request.Tickets
                .Any(u => u.AssigneeId == request.AssignerId);

            if (hasSelfAssignedTicket)
            {
                return Result<int>.Failure(ResponseCode.Conflict, "The assigner and the assignee cannot be the same.");
            }

            userIDs.Add(request.AssignerId);
            foreach (var _v in request.Tickets)
            {
                userIDs.Add(_v.AssigneeId);   
            }
            var existsUserIds =await _userRepository.CheckListUserExistsByUserIDs(userIDs);
            if (!existsUserIds)
            {
                return Result<int>.Failure(ResponseCode.NotFound, "One or more users do not exist.");            }
            var existsTicketIds = await _ticketRepository.CheckListTicketExists(ticketIDs);
            if (!existsTicketIds)
            {
                return Result<int>.Failure(ResponseCode.NotFound, "One or more tickets do not exist.");            }
            
            // Chuyển đổi request thành danh sách AssignedTicket
            var assignedTickets = request.Tickets.Select(ticket => new AssignedTicket
            {
                TicketId = ticket.TicketId,
                AssigneeId = ticket.AssigneeId,
                AssignerId = request.AssignerId
            }).ToList();

            await _assignedTicket.CreateAssignTicketF(assignedTickets);

            return Result<int>.Success(assignedTickets.Count);
        }
        catch (Exception e)
        {
            return Result<int>.Failure(ResponseCode.InternalError, e.Message);
        }
    }
    
    
}