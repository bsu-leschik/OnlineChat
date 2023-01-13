using BusinessLogic.Queries.Users.GetUsernames;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OnlineChat.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-users")]
    public async Task<IActionResult> GetUsers(GetUsernamesQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
}