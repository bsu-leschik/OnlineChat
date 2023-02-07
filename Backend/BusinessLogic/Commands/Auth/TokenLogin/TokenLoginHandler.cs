using BusinessLogic.Services.UsersService;
using Database;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Commands.Auth.TokenLogin;

public class TokenLoginHandler : IRequestHandler<TokenLoginCommand, TokenLoginResponse>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserAccessor _userAccessor;
    private readonly IStorageService _storageService;

    public TokenLoginHandler(IHttpContextAccessor contextAccessor, IUserAccessor userAccessor,
        IStorageService storageService)
    {
        _contextAccessor = contextAccessor;
        _userAccessor = userAccessor;
        _storageService = storageService;
    }

    public async Task<TokenLoginResponse> Handle(TokenLoginCommand request, CancellationToken cancellationToken)
    {
        var id = _userAccessor.GetId();
        var token = _userAccessor.GetToken();
        if (!id.HasValue || !token.HasValue)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.BadRequest);
        }

        var user = await _storageService.GetUsers()
                                        .Where(u => u.Id == id)
                                        .FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return TokenLoginResponse.Error(TokenLoginResponseCode.TokenExpired);
        }

        var principal = _contextAccessor.HttpContext!.User;
        await _contextAccessor.HttpContext.SignInAsync(principal);
        return TokenLoginResponse.Success(user.Username);
    }
}