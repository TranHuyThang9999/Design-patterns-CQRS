using System.ComponentModel.DataAnnotations;
using MediatR;

namespace WebApplicationCQRS.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<Result<int>>
{
    [Required]
    public string Name { get; set; }
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string AvatarUrl { get; set; }


    public CreateUserCommand(string name, string email, string password,string avatarUrl)
    {
        Name = name?.Trim() ?? string.Empty;
        Email = email?.Trim() ?? string.Empty;
        Password = password?.Trim() ?? string.Empty;
        AvatarUrl = avatarUrl?.Trim() ?? string.Empty;
    }
}