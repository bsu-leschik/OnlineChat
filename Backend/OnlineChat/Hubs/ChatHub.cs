using System.Security.Claims;
using BusinessLogic;
using Database;
using Database.Entities;
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
        var user = await UserFinder.FindUser(_storageService, ClaimsPrincipal.Current!, CancellationToken.None);
        if (user is null)
        {
            return new ConnectionResponse(messages: null, ConnectionResponseCode.AccessDenied);
        }

        var chatroom = user.Chatrooms.FirstOrDefault(c => c.Id == id);
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
        await Clients.Group(chatId)
                     .SendCoreAsync("Receive", new object?[] { message });
    }

    public async Task Send(Guid chatId, string message)
    {
        var user = await UserFinder.FindUser(_storageService, ClaimsPrincipal.Current!, CancellationToken.None);

        var chatroom = user?.Chatrooms.FirstOrDefault(c => c.Id == chatId);
        if (chatroom is null)
        {
            return;
        }

        var username = ClaimsPrincipal.Current!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        var messageObject = new Message(username, message);
        await SendMessageToChat(chatId.ToString(), messageObject);
    }
}