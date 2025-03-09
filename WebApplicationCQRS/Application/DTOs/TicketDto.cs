using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApplicationCQRS.Common.Enums;

namespace WebApplicationCQRS.Application.DTOs;

public class CreateTicketDtoRequest
{
    [Required] public string Name { get; set; }
    public string FileDescription { set; get; }

    public string Description { get; set; }
}

public class TicketDtoResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FileDescription { get; set; }

    public string Description { get; set; }
}

public class AssignedTickets
{
    public int Id { get; set; }
    public int AssigneeId { get; set; }
    public string Name { get; set; }
    [JsonIgnore] public string FileDescription { get; set; }

    public string Description { get; set; }

    public DateOnly CreatedAt { get; set; }
}

public class ReceivedAssignedTicketDTO
{
    public int Id { get; set; }
    public string NameUserAssignerIdTicket { get; set; }
    public int AssignedTicketId { get; set; }
    public int TicketId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string FileDescription { get; set; }
    public int AssignerId { get; set; }

    public DateTime TimeAssign { get; set; }
}

public class AssignedTicketDetail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CreatorId { get; set; }
    public string Description { get; set; }

    [JsonIgnore] 
    public string FileDescription { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AssigneeId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string AssigneeName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AssignerId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string AssignerName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssignedTicketStatus? Status { get; set; }

    public DateTime? AssignedAt { get; set; }
}
