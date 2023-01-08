using System.Security.Claims;
using Database;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Commands.Auth.TokenLogin;

public class TokenLoginHandler : IRequestHandler<TokenLoginCommand, TokenLoginResponse>
{
    private readonly IStorageService _storageService;
    private readonly IHttpContextAccessor _contextAccessor;

    public TokenLoginHandler(IStorageService storageService, IHttpContextAccessor contextAccessor)
    {
        _storageService = storageService;
        _contextAccessor = contextAccessor;
    }

    public async Task<TokenLoginResponse> Handle(TokenLoginCommand request, CancellationToken cancellationToken)
    {
        var principal = _contextAccessor.HttpContext?.User;

        if (principal is null)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.BadRequest);
        }

        var (username, token) = Decompose(principal);
        if (username is null)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.BadRequest);
        }

        if (token is null)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.TokenExpired);
        }

        var user = await _storageService.GetUser(u => u.Username == username, CancellationToken.None);
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

    private static (string? Username, Guid? Token) Decompose(ClaimsPrincipal principal)
    {
        Guid? guid = null;

        var username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)
                                ?.Value;
        var guidAsString = principal.Claims.FirstOrDefault(c => c.Type == "Token")?.Value;
        if (Guid.TryParse(guidAsString, out var id))
        {
            guid = id;
        }
        return (username, guid);
    }
}