using MediatR;

namespace BusinessLogic.Commands.Auth.Registration;

public class RegistrationCommand : IRequest<RegistrationResponse>
{
    public string Username { get; set; }
    public string Password { get; set; }
}