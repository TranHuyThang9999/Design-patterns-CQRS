using MediatR;

namespace WebApplicationCQRS.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public CreateUserCommand(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}