using BusinessLogic.Commands.Chatrooms.AddUserToChatroom;
using BusinessLogic.Commands.Chatrooms.CreateChatroom;
using BusinessLogic.Commands.Chatrooms.KickUserFromChatroom;
using BusinessLogic.Commands.Chatrooms.LeaveChatroom;
using BusinessLogic.Commands.Chatrooms.SetUsersRole;
using BusinessLogic.Queries.Chatrooms.GetChatroomInfo;
using BusinessLogic.Queries.Chatrooms.GetChatrooms;
using BusinessLogic.Queries.Messages.GetMessages;
using Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineChat.Controllers;

/// <summary>
/// Controller, responsible for CRUD operations with chatrooms
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    [HttpGet($"{Routes.ChatroomsApi}/get-chatrooms")]
    public async Task<IActionResult> GetChatrooms()
    {
        return Ok(await _mediator.Send(new GetChatroomsRequest()));
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    [HttpGet($"{Routes.ChatroomsApi}/chat-info/{{chatId}}")]
    public async Task<IActionResult> GetChatroomInfo([FromRoute] Guid chatId)
    {
        return Ok(await _mediator.Send(new GetChatroomInfoRequest { ChatId = chatId }));
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    [HttpPost($"{Routes.ChatroomsApi}/create")]
    public async Task<IActionResult> CreateChatroom([FromBody] CreateChatroomCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    [HttpGet($"{Routes.ChatroomsApi}/get-messages/{{chatId}}")]
    public async Task<IActionResult> GetMessages([FromRoute] Guid chatId)
    {
        return Ok(await _mediator.Send(new GetMessagesQuery { ChatId = chatId }));
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    [HttpPost($"{Routes.ChatroomsApi}/add-user")]
    public async Task<IActionResult> AddUserToChatroom(AddUserToChatroomCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    [HttpPost($"{Routes.ChatroomsApi}/kick-user")]
    public async Task<IActionResult> KickUserFromChatroom(KickUserFromChatroomCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    [HttpPost($"{Routes.ChatroomsApi}/leave-chat")]
    public async Task<IActionResult> LeaveChatroom(LeaveChatroomCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    [HttpPost($"{Routes.ChatroomsApi}/set-role")]
    public async Task<IActionResult> SetUsersRole(SetUsersRoleRequest command)
    {
        return Ok(await _mediator.Send(command));
    }
}