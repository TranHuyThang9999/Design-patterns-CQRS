using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplicationCQRS.Common.Enums;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

// == service
namespace WebApplicationCQRS.Application.Features.Users.Commands;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsername(request.Name);
            if (user != null)
            {
                return Result<int>.Failure(ResponseCode.Conflict, "User already exists", HttpStatusCode.Conflict);
            }

            var userModel = new User(request.Name, request.Email,
                BCrypt.Net.BCrypt.HashPassword(request.Password)
                , request.AvatarUrl, DateTime.Now);

            await _unitOfWork.UserRepository.CreateUser(userModel);
            return Result<int>.Success(userModel.Id, "User Created Successfully");
        }
        catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("UQ_UserName") == true)
        {
            return Result<int>.Failure(ResponseCode.Conflict, "User already exists", HttpStatusCode.Conflict);
        }
        catch (Exception e)
        {
            return Result<int>.Failure(ResponseCode.InternalError, "Internal Server Error",
                HttpStatusCode.InternalServerError);
        }
    }
}