using System.Security.Claims;
using BusinessLogic;
using Database;
using Database.Entities;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using OnlineChat.Hubs.Response;

namespace OnlineChat.Hubs;

public class ChatHub : Hub
{
    private readonly IStorageService _storageService;

    public ChatHub(IStorageService storageService)
    {
        _storageService = storageService;
    }

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
        var user = await UserFinder.FindUser(_storageService, Context.User, CancellationToken.None);
        if (user is null)
        {
            return new ConnectionResponse(messages: null, ConnectionResponseCode.AccessDenied);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        var chatroom = await _storageService.GetChatroom(c => c.Id == id, CancellationToken.None);
        return chatroom is null
            ? new ConnectionResponse(messages: null, ConnectionResponseCode.RoomDoesntExist)
            : new ConnectionResponse(messages: chatroom.Messages, ConnectionResponseCode.SuccessfullyConnected);
    }

    public async Task SendFromServer(Guid chatroomId, string message)
    {
        var chatroom = await _storageService.GetChatroom(c => c.Id == chatroomId, CancellationToken.None);
        if (chatroom is null)
        {
            throw new ArgumentException($"{chatroomId} doesn't exist");
        }

        var messageObject = new Message(sender: "Server", text: message);
        await SendMessageToChat(chatroomId.ToString(), messageObject);
    }

    private async Task SendMessageToChat(string chatId, Message message)
    {
        if (!Guid.TryParse(chatId, out var id))
        {
            return;
        }

        var chatroom = await _storageService.GetChatroom(c => c.Id == id, CancellationToken.None);
        if (chatroom is null)
        {
            return;
        }
        chatroom.Messages.Add(message);
        var saving = _storageService.SaveChangesAsync(CancellationToken.None);
        var sending = Clients.Group(chatId)
                     .SendCoreAsync("Receive", new object?[] { message });
        await Task.WhenAll(saving, sending);
    }

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

        var user = await UserFinder.FindUser(_storageService, Context.User, CancellationToken.None);

        var chatroom = user?.Chatrooms.FirstOrDefault(c => c.Id == id);
        if (chatroom is null)
        {
            return;
        }

        var username = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        var messageObject = new Message(username, message);
        await SendMessageToChat(chatId, messageObject);
    }
}