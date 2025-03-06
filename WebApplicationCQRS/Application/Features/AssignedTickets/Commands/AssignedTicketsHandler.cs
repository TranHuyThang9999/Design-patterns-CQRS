using MediatR;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class AssignedTicketsHandler : IRequestHandler<AssignTicketsCommand, Result<int>>
{
    private readonly IAssignedTicket _assignedTicket;

    public AssignedTicketsHandler(IAssignedTicket assignedTicket)
    {
        _assignedTicket = assignedTicket;
    }

    public async Task<Result<int>> Handle(AssignTicketsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 1️⃣ Kiểm tra: Một người nhận nhiều ticket, nhưng không có ticket nào trùng nhau
            var hasDuplicateTickets = request.Tickets
                .GroupBy(t => new { t.AssigneeId, t.TicketId })
                .Any(g => g.Count() > 1);

            if (hasDuplicateTickets)
            {
                return Result<int>.Failure(ResponseCode.Conflict, "Một user không thể nhận cùng một ticket nhiều lần.");
            }

            // 2️⃣ Kiểm tra: Không có 2 user giống nhau trong danh sách (mỗi user chỉ xuất hiện một lần)
            var hasDuplicateAssignee = request.Tickets
                .GroupBy(t => t.AssigneeId)
                .Any(g => g.Count() > 1);

            if (hasDuplicateAssignee)
            {
                return Result<int>.Failure(ResponseCode.Conflict, "Danh sách không được chứa 2 user giống nhau.");
            }

            // 3️⃣ Kiểm tra: AssignerId và AssigneeId không được trùng nhau
            var hasSelfAssignedTicket = request.Tickets
                .Any(t => t.AssigneeId == t.AssignerId);

            if (hasSelfAssignedTicket)
            {
                return Result<int>.Failure(ResponseCode.Conflict, "Người giao và người được giao không thể giống nhau.");
            }

            // Chuyển đổi request thành danh sách AssignedTicket
            var assignedTickets = request.Tickets.Select(ticket => new AssignedTicket
            {
                TicketId = ticket.TicketId,
                AssigneeId = ticket.AssigneeId,
                AssignerId = ticket.AssignerId
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