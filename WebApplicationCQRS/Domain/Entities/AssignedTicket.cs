using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationCQRS.Domain.Entities;

public class AssignedTicket : BaseEntity
{
    public AssignedTicket(int ticketId, int userId, Ticket ticket, User user)
    {
        TicketId = ticketId;
        UserId = userId;
        Ticket = ticket;
        User = user;
    }
    public AssignedTicket(){}
    public int TicketId { get; set; }

    public int UserId { get; set; }

    [ForeignKey("TicketId")]
    public Ticket Ticket { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}