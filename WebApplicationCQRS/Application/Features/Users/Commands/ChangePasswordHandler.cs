using System.Net;
using MediatR;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Users.Commands;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result<DateTime>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ChangePasswordHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DateTime>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetUserById(request.UserId);
            if (user == null)
            {
                return Result<DateTime>.Failure(ResponseCode.InternalError,
                    $"User with id {request.UserId} was not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Password))
            {
                return Result<DateTime>.Failure(ResponseCode.Conflict, "User not found", HttpStatusCode.NotFound);
            }

            var model = new UserUpdateProfile();
            model.Id = request.UserId;
            model.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            var status = await _unitOfWork.UserRepository.UpdateUserById(model);
            return Result<DateTime>.Success(DateTime.Now);
        }

        catch (Exception e)
        {
            return Result<DateTime>.Failure(ResponseCode.InternalError, "Internal Server Error",
                HttpStatusCode.InternalServerError);
        }
    }
}