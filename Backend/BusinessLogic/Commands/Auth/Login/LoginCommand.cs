using MediatR;

namespace BusinessLogic.Commands.Auth.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public string Username { get; set; }
    public string Password { get; set; }
}