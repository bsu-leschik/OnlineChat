using BusinessLogic.Commands.Auth.Registration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineChat.Controllers;

/// <summary>
/// Registration controller
/// </summary>
[ApiController]
[Route("api/register")]
public class RegistrationController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegistrationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegistrationCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}