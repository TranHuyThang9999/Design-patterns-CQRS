using MediatR;

namespace WebApplicationCQRS.Application.Features.Users.Commands
{
    public class UpdateProfileCommand : IRequest<Result<DateTime>>
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }

        public UpdateProfileCommand()
        {
        }

        public UpdateProfileCommand(int userId, string email, string avatarUrl)
        {
            UserId = userId;
            Email = email;
            AvatarUrl = avatarUrl?.Trim() ?? string.Empty;
        }
    }
}