using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationCQRS.Domain.Entities;

public class AssignedTicket : BaseEntity
{
    public AssignedTicket(int ticketId, int assigneeId, int assignerId, Ticket ticket, User assignee, User assigner)
    {
        TicketId = ticketId;
        AssigneeId = assigneeId;
        AssignerId = assignerId;
        Ticket = ticket;
        Assignee = assignee;
        Assigner = assigner;
    }
    
    public AssignedTicket() { }

    public int TicketId { get; set; }

    public int AssigneeId { get; set; } // Người được giao ticket

    public int AssignerId { get; set; } // Người giao ticket

    [ForeignKey("TicketId")]
    public Ticket Ticket { get; set; }

    [ForeignKey("AssigneeId")]
    public User Assignee { get; set; }  // Người nhận ticket

    [ForeignKey("AssignerId")]
    public User Assigner { get; set; }  // Người giao ticket
}