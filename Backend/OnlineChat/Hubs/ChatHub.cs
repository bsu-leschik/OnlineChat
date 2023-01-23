using BusinessLogic.Commands.Chatrooms.AddUserToChatroom;
using BusinessLogic.Commands.Chatrooms.KickUserFromChatroom;
using BusinessLogic.Commands.Chatrooms.LeaveChatroom;
using BusinessLogic.UsersService;
using Constants;
using Database;
using Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace OnlineChat.Hubs;

/// <summary>
/// RPC Chatroom hub
/// </summary>
public class ChatHub : Hub
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;
    private readonly IMediator _mediator;

    public ChatHub(IStorageService storageService, IUsersService usersService, IMediator mediator)
    {
        _storageService = storageService;
        _usersService = usersService;
        _mediator = mediator;
    }

    /// <summary>
    /// First method to invoke
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    public async Task<ConnectionResponse> Connect(string chatId)
    {
        if (!Guid.TryParse(chatId, out var id))
        {
            return new ConnectionResponse(messages: null, ConnectionResponseCode.Error);
        }
        if (Context.User is null)
        {
            return new ConnectionResponse(messages: null, ConnectionResponseCode.AccessDenied);
        }
        var user = await _usersService.FindUser(Context.User, CancellationToken.None);
        if (user is null)
        {
            return new ConnectionResponse(messages: null, ConnectionResponseCode.AccessDenied);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        var chatroom = await _storageService.GetChatroomAsync(c => c.Id == id, CancellationToken.None);
        return chatroom is null
            ? new ConnectionResponse(messages: null, ConnectionResponseCode.RoomDoesntExist)
            : new ConnectionResponse(messages: chatroom.Messages, ConnectionResponseCode.SuccessfullyConnected);
    }

    /// <summary>
    /// Method for sending message to chat
    /// Warning: method doesn't check if the caller is in chat  
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="message"></param>
    private async Task SendMessageToChat(Guid chatId, Message message)
    {
        var chatroom = await _storageService.GetChatroomAsync(c => c.Id == chatId, CancellationToken.None);
        if (chatroom is null)
        {
            return;
        }
        chatroom.AddMessage(message);
        var saving = _storageService.SaveChangesAsync(CancellationToken.None);
        var sending = Clients.Group(chatId.ToString())
                             .SendCoreAsync("Receive", new object?[] { message });
        await Task.WhenAll(saving, sending);
    }

    /// <summary>
    /// Method for users to send messages
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="message"></param>
    public async Task Send(string chatId, string message)
    {
        if (!Guid.TryParse(chatId, out var id))
        {
            return;
        }

        if (Context.User is null)
        {
            return;
        }

        var user = await _usersService.FindUser(Context.User, CancellationToken.None);

        var chatroom = user?.Chatrooms.FirstOrDefault(c => c.Id == id);
        if (chatroom is null)
        {
            return;
        }

        var username = Context.User.Claims.FirstOrDefault(c => c.Type == Claims.Name)!.Value;
        var messageObject = new Message(username, message, id);
        await SendMessageToChat(id, messageObject);
    }

    public async Task<AddUserToChatroomResponse> AddUserToChatroom(string username, Guid chatId)
    {
        var result = await _mediator.Send(new AddUserToChatroomCommand { ChatId = chatId, Username = username });
        if (result != AddUserToChatroomResponse.Success)
        {
            return result;
        }

        await SendMessageToChat(chatId, new Message("", $"User {username} joined the chat", chatId));
        return result;
    }

    public async Task<KickUserFromChatroomResponse> KickUserFromChatroom(string username, Guid chatId)
    {
        var result = await _mediator.Send(new KickUserFromChatroomCommand { ChatId = chatId, Username = username });
        if (result != KickUserFromChatroomResponse.Success)
        {
            return result;
        }
        
        await SendMessageToChat(chatId, new Message("", $"User {username} was kicked from the chat", chatId));
        return result;
    }

    public async Task<LeaveChatroomResponse> LeaveChatroom(Guid chatId)
    {
        var result = await _mediator.Send(new LeaveChatroomCommand { ChatId = chatId });
        if (result != LeaveChatroomResponse.Success)
        {
            return result;
        }

        var (username, _) = await _usersService.Decompose(Context.User!, CancellationToken.None);
        await SendMessageToChat(chatId, new Message("", $"User {username} left the chat", chatId));
        return result;
    }
}