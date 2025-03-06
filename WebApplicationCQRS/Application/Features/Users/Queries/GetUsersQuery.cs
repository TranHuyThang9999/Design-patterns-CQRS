using MediatR;
using WebApplicationCQRS.Application.DTOs;

namespace WebApplicationCQRS.Application.Features.Users.Queries;

public class GetUsersQuery : IRequest<Result<List<UserDto>>>
{
    public int CurrentUserId { get; set; }
    public GetUsersQuery()
    {
    }

    public GetUsersQuery(int currentUserId)
    {
        CurrentUserId = currentUserId;
    }
}