using BusinessLogic.Services.UsersService;
using Database;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Commands.Auth.TokenLogin;

public class TokenLoginHandler : IRequestHandler<TokenLoginCommand, TokenLoginResponse>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUsersService _usersService;

    public TokenLoginHandler(IStorageService storageService, IHttpContextAccessor contextAccessor, IUsersService usersService)
    {
        _contextAccessor = contextAccessor;
        _usersService = usersService;
    }

    public async Task<TokenLoginResponse> Handle(TokenLoginCommand request, CancellationToken cancellationToken)
    {
        var principal = _contextAccessor.HttpContext!.User;
        var user = await _usersService.GetCurrentUser(cancellationToken);
        if (user is null)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.TokenExpired);
        }
        
        await _contextAccessor.HttpContext!.SignInAsync(principal);
        return TokenLoginResponse.Success(user.Username);
    }
}