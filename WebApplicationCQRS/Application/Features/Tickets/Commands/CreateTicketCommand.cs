using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;

namespace WebApplicationCQRS.Application.Features.Tickets.Commands;

public class CreateTicketCommand : IRequest<Result<int>>
{
    public int CreatorId { get; set; }
    public string Name { get; set; }
    public string FileDescription { set; get; }
    public CreateTicketCommand(){}
    public CreateTicketCommand(int creatorId,string name, string fileDescription)
    {
        CreatorId = creatorId;
        Name = name?.Trim() ?? string.Empty;
        FileDescription = fileDescription;
    }
}