using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Users.Queries;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.Id);
        if (user is null)
        {
            return null;
        }

        return new UserDto(id:user.Id, name:user.Name, email:user.Email);
    }
}