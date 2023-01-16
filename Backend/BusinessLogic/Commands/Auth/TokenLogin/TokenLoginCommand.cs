using MediatR;

namespace BusinessLogic.Commands.Auth.TokenLogin;

public struct TokenLoginCommand : IRequest<TokenLoginResponse> {}