using BusinessLogic.Services;
using BusinessLogic.Services.UsersService;
using Constants;
using Database;
using Entities;
using Entities.Chatrooms;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Hubs.Chat;

/// <summary>
/// RPC Chatroom hub
/// </summary>
[Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
public class ChatHub : Hub<IChatClientInterface>
{
    private readonly IStorageService _storageService;
    private readonly IUserAccessor _userAccessor;
    private readonly IUserConnectionIdTracker _idTracker;

    public ChatHub(IStorageService storageService, IUserAccessor userAccessor, IUserConnectionIdTracker idTracker)
    {
        _storageService = storageService;
        _userAccessor = userAccessor;
        _idTracker = idTracker;
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    public async Task<ConnectionResponseCode> Connect(string chatroomId)
    {
        if (!Guid.TryParse(chatroomId, out var chatId))
        {
            return ConnectionResponseCode.Error;
        }
        var id = _userAccessor.GetId()!;
        var ticket = await _storageService.GetChatroomTickets()
                                    .Where(t => t.UserId == id && t.ChatroomId == chatId)
                                    .Include(ticket => ticket.Chatroom)
                                    .FirstOrDefaultAsync(CancellationToken.None);

        if (ticket is null)
        {
            return ConnectionResponseCode.AccessDenied;
        }

        ticket.LastMessageRead = ticket.Chatroom.MessagesCount;
        var saving = _storageService.SaveChangesAsync(CancellationToken.None);
        var adding = Groups.AddToGroupAsync(connectionId: Context.ConnectionId,
            groupName: chatId.ToString());
        await Task.WhenAll(saving, adding);
        return ConnectionResponseCode.SuccessfullyConnected;
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    public Task Disconnect(Guid chatId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    /// <summary>
    /// Method for users to send messages
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="message"></param>
    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    public async Task Send(Guid chatId, string message)
    {
        var username = _userAccessor.GetUsername()!;
        var chatroom = await _storageService.GetChatrooms()
                                            .Where(c => c.Id == chatId)
                                            .Include(c => c.Messages.Take(0))
                                            .Include(c => c.UserTickets)
                                            .ThenInclude(t => t.User)
                                            .FirstOrDefaultAsync(CancellationToken.None);
        if (chatroom is null || !chatroom.Users.Contains(u => u.Username == username))
        {
            return;
        }

        var messageObject = new Message(username, message);
        await SendMessageToChat(chatroom, messageObject);
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    public override async Task OnConnectedAsync()
    {
        _idTracker.Add(username: _userAccessor.GetUsername()!, connectionId: Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    [Authorize(AuthenticationSchemes = Schemes.DefaultCookieScheme)]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _idTracker.Remove(_userAccessor.GetUsername()!);
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Method for sending message to chat
    /// Warning: method doesn't check if the caller is in chat  
    /// </summary>
    /// <param name="chatroom"></param>
    /// <param name="message"></param>
    private async Task SendMessageToChat(Chatroom chatroom, Message message)
    {
        chatroom.AddMessage(message);
        var count = chatroom.MessagesCount;
        var usersOnlineInChat = chatroom.Users
                                        .Where(u => _idTracker.GetConnectionId(u.Username) is not null)
                                        .Select(u => u.Id)
                                        .ToList();
        var updating = _storageService.GetChatroomTickets()
                                      .Where(id =>
                                          id.ChatroomId == chatroom.Id && usersOnlineInChat.Any(u => u == id.UserId))
                                      .ForEachAsync(t => t.LastMessageRead = count, CancellationToken.None);
        var sending = Clients
                      .Group(chatroom.Id.ToString())
                      .Receive(message, chatroom.Id.ToString());
        var promoting = PromoteChatToTop(chatroom);
        await Task.WhenAll(sending, promoting, updating);
        await _storageService.SaveChangesAsync(CancellationToken.None);
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

public enum ConnectionResponseCode
{
    SuccessfullyConnected = 0,
    AccessDenied,
    Error,
    RoomDoesntExist
}