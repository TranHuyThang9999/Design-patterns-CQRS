using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationCQRS.Domain.Entities;

/// <summary>
/// Bảng lưu lịch sử reassignment của ticket.
/// Khi một ticket được reassigned từ người này sang người khác,
/// hệ thống sẽ lưu lại thông tin này để tracking.
/// </summary>
public class HistoryAssignTicket : BaseEntity
{
    public HistoryAssignTicket(int assignedTicketId, int previousAssigneeId, int newAssigneeId, AssignedTicket assignedTicket, User previousAssignee, User newAssignee)
    {
        AssignedTicketId = assignedTicketId;
        PreviousAssigneeId = previousAssigneeId; // Sửa lỗi gán đúng kiểu dữ liệu
        NewAssigneeId = newAssigneeId;
        AssignedTicket = assignedTicket;
        PreviousAssignee = previousAssignee;
        NewAssignee = newAssignee;
    }
    
    public HistoryAssignTicket() {}

    /// <summary>
    /// ID của bản ghi trong bảng AssignedTickets.
    /// </summary>
    public int AssignedTicketId { get; set; }

    /// <summary>
    /// ID của người mới được assign ticket.
    /// </summary>
    public int NewAssigneeId { get; set; }

    /// <summary>
    /// ID của người đã assign trước đó (người cũ).
    /// </summary>
    public int PreviousAssigneeId { get; set; }

    /// <summary>
    /// Thông tin bản ghi assign tương ứng trong bảng AssignedTickets.
    /// </summary>
    [ForeignKey("AssignedTicketId")]
    public AssignedTicket AssignedTicket { get; set; }

    /// <summary>
    /// Thông tin người được assign trước đó (người cũ).
    /// </summary>
    [ForeignKey("PreviousAssigneeId")]
    public User PreviousAssignee { get; set; }

    /// <summary>
    /// Thông tin người mới được assign ticket.
    /// </summary>
    [ForeignKey("NewAssigneeId")]
    public User NewAssignee { get; set; }
}