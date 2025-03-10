using System.Net;
using MediatR;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Users.Commands;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Result<DateTime>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileHandler()
    {
    }

    public UpdateProfileHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<DateTime>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var model = new UserUpdateProfile();
            model.Id = request.UserId;
            model.Email = request.Email;
            model.AvatarUrl = request.AvatarUrl;
            var status = await _unitOfWork.UserRepository.UpdateUserById(model);
            if (!status)
            {
                return Result<DateTime>.Failure(ResponseCode.InternalError,
                    $"User with id {request.UserId} was not found");
            }

            return Result<DateTime>.Success(DateTime.Now);
        }
        catch (Exception e)
        {
            return Result<DateTime>.Failure(ResponseCode.InternalError, "Internal Server Error",
                HttpStatusCode.InternalServerError);
        }
    }
}