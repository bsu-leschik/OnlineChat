using BusinessLogic.Commands.Auth.Login;
using BusinessLogic.Commands.Auth.Logout;
using BusinessLogic.Commands.Auth.TokenLogin;
using Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineChat.Controllers;

/// <summary>
/// Controller, responsible for authentication (login, logout, auto-login)
/// </summary>
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet($"{Routes.AuthenticationApi}/auto-login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginWithToken()
    {
        return Ok(await _mediator.Send(new TokenLoginCommand()));
    }
    
    [HttpPost($"{Routes.AuthenticationApi}/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPost($"{Routes.AuthenticationApi}/logout")]
    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand());
        return Ok();
    }
}