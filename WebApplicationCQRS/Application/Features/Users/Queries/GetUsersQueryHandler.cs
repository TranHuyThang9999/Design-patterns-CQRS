using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Users.Queries;

public class GetUsersQueryHandler :IRequestHandler<GetUsersQuery, Result<List<UserDto>>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<UserDto>users = new List<UserDto>();
            var usersRepo = await _userRepository.GetActiveUsers(request.CurrentUserId);
            foreach (var VARIABLE in usersRepo)
            {
                
                users.Add(new UserDto()
                {
                    Id = VARIABLE.Id,
                    Email = VARIABLE.Email,
                    Name = VARIABLE.Name,
                    AvatarUrl = VARIABLE.AvatarUrl
                    
                });
            }
            return Result<List<UserDto>>.Success(users);
        }
        catch (Exception e)
        {
            return Result<List<UserDto>>.Failure(ResponseCode.InternalError,e.Message);
            throw;
        }
    }
}