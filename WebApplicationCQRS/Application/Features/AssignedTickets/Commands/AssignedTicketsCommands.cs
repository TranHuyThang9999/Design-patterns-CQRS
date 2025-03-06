using MediatR;
using WebApplicationCQRS.Application.DTOs;

namespace WebApplicationCQRS.Application.Features.AssignedTickets.Commands;

public class AssignTicketsCommand : IRequest<Result<int>>
{
   public List<AssignedTicketDto> Tickets { get; set; }
   
   public AssignTicketsCommand(){}
   
   public AssignTicketsCommand(List<AssignedTicketDto> tickets)
   {
      Tickets = tickets;
   }
}