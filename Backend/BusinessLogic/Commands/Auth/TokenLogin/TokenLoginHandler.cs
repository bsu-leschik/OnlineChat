using BusinessLogic.UsersService;
using Database;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Commands.Auth.TokenLogin;

public class TokenLoginHandler : IRequestHandler<TokenLoginCommand, TokenLoginResponse>
{
    private readonly IStorageService _storageService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUsersService _usersService;

    public TokenLoginHandler(IStorageService storageService, IHttpContextAccessor contextAccessor, IUsersService usersService)
    {
        _storageService = storageService;
        _contextAccessor = contextAccessor;
        _usersService = usersService;
    }

    public async Task<TokenLoginResponse> Handle(TokenLoginCommand request, CancellationToken cancellationToken)
    {
        var principal = _contextAccessor.HttpContext?.User;

        if (principal is null)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.BadRequest);
        }

        var (username, token) = await _usersService.Decompose(principal, cancellationToken);
        if (username is null)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.BadRequest);
        }

        if (token is null)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.TokenExpired);
        }

        var user = await _storageService.GetUserAsync(u => u.Username == username, CancellationToken.None);
        if (user is null)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.BadRequest);
        }

        if (user.Token != token)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.TokenExpired);
        }

        await _contextAccessor.HttpContext!.SignInAsync(principal);
        return TokenLoginResponse.Success(username);
    }
}