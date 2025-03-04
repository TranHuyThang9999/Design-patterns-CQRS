using System.Net;
using MediatR;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

namespace WebApplicationCQRS.Application.Features.Users.Commands;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Result<DateTime>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateProfileHandler> _logger;

    public UpdateProfileHandler()
    {
    }

    public UpdateProfileHandler(IUserRepository userRepository, ILogger<UpdateProfileHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<DateTime>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var model = new UserUpdateProfile();
            model.Id = request.UserId;
            model.Email = request.Email;
            model.AvatarUrl = request.AvatarUrl;
            var status = await _userRepository.UpdateUserById(model);
            if (!status)
            {
                _logger.LogError($"User with id {request.UserId} was not found");
                return Result<DateTime>.Failure(ResponseCode.InternalError,
                    $"User with id {request.UserId} was not found");
            }

            return Result<DateTime>.Success(DateTime.Now);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result<DateTime>.Failure(ResponseCode.InternalError, "Internal Server Error",
                HttpStatusCode.InternalServerError);
        }
    }
}