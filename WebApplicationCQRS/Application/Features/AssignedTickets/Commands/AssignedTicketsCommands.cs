using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplicationCQRS.Application.DTOs;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class AssignTicketsCommand : IRequest<Result<int>>
{
    [JsonIgnore] [BindNever] public int AssignerId { get; set; }
    public AssignedTicketDto Tickets { get; set; }

    public AssignTicketsCommand()
    {
    }

    public AssignTicketsCommand(int assignerId, AssignedTicketDto tickets)
    {
        AssignerId = assignerId;
        Tickets = tickets;
    }
}