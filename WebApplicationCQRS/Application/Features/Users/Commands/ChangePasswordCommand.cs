using MediatR;

namespace WebApplicationCQRS.Application.Features.Users.Commands;

public class ChangePasswordCommand : IRequest<Result<DateTime>>
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }

    public ChangePasswordCommand(int userId, string currentPassword, string newPassword)
    {
        UserId = userId;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }
}