using MediatR;
using WebApplicationCQRS.Domain.Entities;
using WebApplicationCQRS.Domain.Interfaces;
// == service
namespace WebApplicationCQRS.Application.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _userRepository;
    

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var id = Random.Shared.Next(1, 100);
        var user = new User(id, request.Name, request.Email,
            BCrypt.Net.BCrypt.HashPassword(request.Password),
            DateOnly.FromDateTime(DateTime.Now),
            DateTime.Now, DateTime.Now);
        
        await _userRepository.CreateUser(user);
        return user.Id;
    }
}