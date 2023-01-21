using BusinessLogic.UsersService;
using Constants;
using Database;
using Database.Entities;
using Microsoft.AspNetCore.SignalR;

namespace OnlineChat.Hubs;

/// <summary>
/// RPC Chatroom hub
/// </summary>
public class ChatHub : Hub
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;

    public ChatHub(IStorageService storageService, IUsersService usersService)
    {
        _storageService = storageService;
        _usersService = usersService;
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
        chatroom.Messages.Add(message);
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
        var messageObject = new Message(username, message);
        await SendMessageToChat(id, messageObject);
    }
}