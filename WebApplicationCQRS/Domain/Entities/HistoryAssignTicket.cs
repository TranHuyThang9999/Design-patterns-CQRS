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
    /// <summary>
    /// ID của bản ghi trong bảng AssignedTickets.
    /// </summary>
    public int AssignedTicketId { get; set; }

    /// <summary>
    /// ID của người được assign trước đó (người cũ).
    /// </summary>
    public int OldAssigneeId { get; set; }

    /// <summary>
    /// ID của người mới được assign ticket.
    /// </summary>
    public int NewAssigneeId { get; set; }
    // 🛠 Navigation Properties - Dùng để truy xuất dữ liệu liên kết

    /// <summary>
    /// Thông tin bản ghi assign tương ứng trong bảng AssignedTickets.
    /// </summary>
    [ForeignKey("AssignedTicketId")]
    public AssignedTicket AssignedTicket { get; set; }

    /// <summary>
    /// Thông tin người được assign trước đó (người cũ).
    /// </summary>
    [ForeignKey("OldAssigneeId")]
    public User OldAssignee { get; set; }

    /// <summary>
    /// Thông tin người mới được assign ticket.
    /// </summary>
    [ForeignKey("NewAssigneeId")]
    public User NewAssignee { get; set; }
}