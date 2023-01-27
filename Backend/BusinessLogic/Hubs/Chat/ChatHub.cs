using BusinessLogic.UsersService;
using Constants;
using Database;
using Entities;
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

    public ChatHub(IStorageService storageService, IUsersService usersService)
    {
        _storageService = storageService;
        _usersService = usersService;
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
        
        await Task.WhenAll(saving, sending);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
}

public static class ChatHubExtensions
{
    public static Task NotifyUserAdded(this IHubContext<ChatHub> context, string chatId, string username, CancellationToken cancellationToken)
    {
        return NotifyAll(context, chatId, $"User {username} joined the chat", cancellationToken);
    }

    public static Task NotifyUserKicked(this IHubContext<ChatHub> context, string chatId, string username, CancellationToken cancellationToken)
    {
        return NotifyAll(context, chatId, $"User {username} was kicked from the chat", cancellationToken);
    }
    
    public static Task NotifyUserLeft(this IHubContext<ChatHub> context, string chatId, string username, CancellationToken cancellationToken)
    {
        return NotifyAll(context, chatId, $"User {username} left the chat", cancellationToken);
    }

    private static Task NotifyAll(this IHubContext<ChatHub> context, string chatId, string message, CancellationToken cancellationToken)
    {
        return context.Clients.Group(chatId).SendCoreAsync(nameof(IChatClientInterface.Receive),
            new object?[] { new Message("", message) }, cancellationToken);
    }
}