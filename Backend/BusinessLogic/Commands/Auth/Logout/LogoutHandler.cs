using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Commands.Auth.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IHttpContextAccessor _accessor;

    public LogoutHandler(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _accessor.HttpContext!.SignOutAsync();
        return default;
    }
}