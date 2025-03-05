using MediatR;
using WebApplicationCQRS.Application.DTOs;

namespace WebApplicationCQRS.Application.Features.Users.Queries;

public class GetUsersQuery : IRequest<Result<List<UserDto>>>
{
    public GetUsersQuery()
    {
    }
}