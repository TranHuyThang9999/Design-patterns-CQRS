using MediatR;
using WebApplicationCQRS.Application.DTOs;

namespace WebApplicationCQRS.Application.Features.Users.Queries;

public class GetUserQuery :IRequest<Result<UserDto>>
{
    public int Id { get; set; }
    public GetUserQuery(int id)
    {
        Id = id;
    }
    
}