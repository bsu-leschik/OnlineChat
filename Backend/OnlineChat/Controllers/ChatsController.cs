using BusinessLogic.Commands.CreateChatroom;
using BusinessLogic.Queries.Chatrooms.GetChatrooms;
using BusinessLogic.Queries.Messages.GetMessages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineChat.Controllers;

[ApiController]
[Route("api/chatrooms")]
[Authorize(AuthenticationSchemes = Constants.Schemes.DefaultCookieScheme)]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(AuthenticationSchemes = Constants.Schemes.DefaultCookieScheme)]
    [HttpGet("get-chatrooms")]
    public async Task<IActionResult> GetChatrooms()
    {
        return Ok(await _mediator.Send(new GetChatroomsRequest()));
    }

    [Authorize(AuthenticationSchemes = Constants.Schemes.DefaultCookieScheme)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateChatroom([FromBody] CreateChatroomCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(AuthenticationSchemes = Constants.Schemes.DefaultCookieScheme)]
    [HttpGet("get-messages")]
    public async Task<IActionResult> GetMessages([FromBody] GetMessagesQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
}