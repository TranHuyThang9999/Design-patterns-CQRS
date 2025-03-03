using MediatR;

namespace WebApplicationCQRS.Application.Features.Users.Queries;

public class LoginQuery : IRequest<Result<string>>
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public LoginQuery(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}