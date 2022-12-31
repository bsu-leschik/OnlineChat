using BusinessLogic.Commands.CreateChatroom;
using BusinessLogic.Queries.Chatrooms.GetChatrooms;
using BusinessLogic.Queries.Messages.GetMessages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OnlineChat.Controllers;

[ApiController]
[Route("api/chatrooms/")]
public class ChatsController : Controller
{
    private readonly IMediator _mediator;

    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetChatrooms()
    {
        return Ok(await _mediator.Send(new GetChatroomsRequest()));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateChatroom(CreateChatroomCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpGet("messages")]
    public async Task<IActionResult> GetMessages(GetMessagesQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
}