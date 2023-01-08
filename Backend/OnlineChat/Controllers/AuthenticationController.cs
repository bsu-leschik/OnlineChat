using BusinessLogic.Commands.Auth.Login;
using BusinessLogic.Commands.Auth.Logout;
using BusinessLogic.Commands.Auth.TokenLogin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineChat.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/auto-login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login()
    {
        return Ok(await _mediator.Send(new TokenLoginCommand()));
    }
    
    [HttpPost("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPost("/logout")]
    [Authorize(AuthenticationSchemes = Constants.Schemes.DefaultCookieScheme)]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand());
        return Ok();
    }
}