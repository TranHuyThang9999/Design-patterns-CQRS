using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Users.Queries;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.Id);
        if (user is null)
        {
            return null;
        }

        return Result<UserDto>.Success(new UserDto(user.Id, user.Name, user.Email,user.AvatarUrl));
    }
}