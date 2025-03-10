using MediatR;
using WebApplicationCQRS.Application.DTOs;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Users.Queries;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetUserById(request.Id);
        if (user is null)
        {
            return null;
        }
        
        return Result<UserDto>.Success(new UserDto(user.Id, user.Name, user.Email, user.AvatarUrl));
    }
}