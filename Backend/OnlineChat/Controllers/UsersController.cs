using BusinessLogic.Queries.Users.GetUsernames;
using Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OnlineChat.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet($"{Routes.UsersApi}/get-users/")]
    public Task<IActionResult> GetUsers()
    {
        return GetUsers("");
    }
    
    [HttpGet($"{Routes.UsersApi}/get-users/{{startingWith}}")]
    public async Task<IActionResult> GetUsers([FromRoute] string startingWith)
    {
        return Ok(await _mediator.Send(new GetUsernamesQuery
                                           {
                                               StartingWith = string.IsNullOrEmpty(startingWith)
                                                   ? null
                                                   : startingWith
                                           }));
    }
}