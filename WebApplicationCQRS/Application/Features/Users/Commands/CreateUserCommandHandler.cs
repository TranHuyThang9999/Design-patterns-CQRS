using MediatR;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;

// == service
namespace WebApplicationCQRS.Application.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<int>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(IUserRepository userRepository, ILogger<CreateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }


    public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = Random.Shared.Next(1, 100);
            var user = new User(id, request.Name, request.Email,
                BCrypt.Net.BCrypt.HashPassword(request.Password),
                DateOnly.FromDateTime(DateTime.Now),
                DateTime.Now, DateTime.Now);

            await _userRepository.CreateUser(user);
            return Result<int>.Success(user.Id, "User Created Successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}