using MediatR;

namespace BusinessLogic.Commands.Auth.TokenLogin;

public class TokenLoginCommand : IRequest<TokenLoginResponse>
{
}