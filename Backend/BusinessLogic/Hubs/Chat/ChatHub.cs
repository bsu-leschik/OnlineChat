using BusinessLogic.Services;
using BusinessLogic.Services.UsersService;
using Constants;
using Database;
using Entities;
using Entities.Chatrooms;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BusinessLogic.Hubs.Chat;

/// <summary>
/// RPC Chatroom hub
/// </summary>
[Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
public class ChatHub : Hub<IChatClientInterface>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;
    private readonly IUserConnectionIdTracker _idTracker;

    public ChatHub(IStorageService storageService, IUsersService usersService, IUserConnectionIdTracker idTracker)
    {
        _storageService = storageService;
        _usersService = usersService;
        _idTracker = idTracker;
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    public async Task<ConnectionResponseCode> Connect(Guid chatId)
    {
        var chat = await _storageService.GetChatroomAsync(c => c.Id == chatId, CancellationToken.None);
        if (chat is null)
        {
            return ConnectionResponseCode.RoomDoesntExist;
        }

        var username = _usersService.GetUsername()!;
        if (!chat.Users.Contains(u => u.Username == username))
        {
            return ConnectionResponseCode.AccessDenied;
        }

        await Groups.AddToGroupAsync(connectionId: Context.ConnectionId,
            groupName: chatId.ToString());
        return ConnectionResponseCode.SuccessfullyConnected;
    }

    /// <summary>
    /// Method for users to send messages
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="message"></param>
    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
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

        var user = await _usersService.GetCurrentUser(CancellationToken.None);
        var chatroom = user?.Chatrooms.FirstOrDefault(c => c.Id == id);
        if (chatroom is null)
        {
            return;
        }

        var username = Context.User.Claims.FirstOrDefault(c => c.Type == Claims.Name)!.Value;
        var messageObject = new Message(username, message);
        await SendMessageToChat(id, messageObject);
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    public override async Task OnConnectedAsync()
    {
        _idTracker.Add(username: _usersService.GetUsername()!, connectionId: Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _idTracker.Remove(_usersService.GetUsername()!);
        await base.OnDisconnectedAsync(exception);
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
        var sending = Clients
                      .Group(chatId.ToString())
                      .Receive(message, chatroom.Id.ToString());
        var promoting = PromoteChatToTop(chatroom);

        await Task.WhenAll(saving, sending, promoting);
    }

    private async Task PromoteChatToTop(Chatroom chatroom)
    {
        List<string> connectionIds = chatroom.Users
                                             .Select(u => u.Username)
                                             .Select(n => _idTracker.GetConnectionId(n))
                                             .Where(c => c is not null)
                                             .ToList()!;

        await Clients.Clients(connectionIds).PromoteToTop(chatroom.Id.ToString());
    }
}

public static class ChatHubExtensions
{
    public static Task NotifyUserAdded(this IHubContext<ChatHub> context, string chatId, string username,
        CancellationToken cancellationToken)
    {
        return NotifyAll(context, chatId, $"User {username} joined the chat", cancellationToken);
    }

    public static Task NotifyUserKicked(this IHubContext<ChatHub> context, string chatId, string username,
        CancellationToken cancellationToken)
    {
        return NotifyAll(context, chatId, $"User {username} was kicked from the chat", cancellationToken);
    }

    public static Task NotifyUserLeft(this IHubContext<ChatHub> context, string chatId, string username,
        CancellationToken cancellationToken)
    {
        return NotifyAll(context, chatId, $"User {username} left the chat", cancellationToken);
    }

    private static Task NotifyAll(this IHubContext<ChatHub> context, string chatId, string message,
        CancellationToken cancellationToken)
    {
        return context.Clients.Group(chatId).SendCoreAsync(nameof(IChatClientInterface.Receive),
            new object?[] { new Message("", message) }, cancellationToken);
    }
}