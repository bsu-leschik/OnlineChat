using Microsoft.AspNetCore.SignalR;
using OnlineChat.Hubs.Reponse;
using OnlineChat.Models;
using OnlineChat.Services;

namespace OnlineChat.Hubs;

public class ChatHub : Hub
{
    private readonly Storage _storage;


    public ChatHub(Storage storage)
    {
        _storage = storage;
    }

    public async Task SendFromServer(int chatroomId, string message)
    {
        var m = new Message("Server", message);
        _storage.GetChatroomById(chatroomId)?.Messages.Add(m);
        await Clients.Group(chatroomId.ToString()).SendCoreAsync("Receive", new object?[]
                                                    { m });
    }

    public async Task<ConnectionResponse> Connect(string username, int chatId)
    {
        if (string.IsNullOrEmpty(username) || chatId < 0)
        {
            return new ConnectionResponse
            {
                Response = ConnectionResponseCode.WrongNickname
            };
        }

        if (_storage.GetChatroomById(chatId) is null)
        {
            return new ConnectionResponse
            { 
                Response = ConnectionResponseCode.RoomDoesntExist 
            };
        }
        var user = new User(username, Context.ConnectionId, chatId);
        _storage.AddUser(user, chatId);
        await SendFromServer(chatId, $"{username} joined the conversation");
        await Groups.AddToGroupAsync(user.ConnectionId, chatId.ToString());
        return new ConnectionResponse
        { 
          Messages = _storage.GetChatroomById(chatId)?.Messages,
          Response = ConnectionResponseCode.SuccessfullyConnected 
        };
    }

    public async Task Send(string message)
    {
        var sender = _storage.GetUser(Context.ConnectionId);
        if (sender is null)
        {
            return;
        }

        var chatroom = _storage.GetChatroomById(sender.ChatroomId);
        if (chatroom is null)
        {
            return;
        }

        chatroom.Messages.Add(new Message(sender.Nickname, message));
        await Clients.Group(sender.ChatroomId.ToString()).SendCoreAsync("Receive",
            new object?[] { new Message(sender.Nickname, message) });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var sender = _storage.GetUser(Context.ConnectionId);
        if (sender is null)
        {
            return;
        }

        await SendFromServer(sender.ChatroomId, $"{sender.Nickname} left");
        _storage.Remove(sender);
        await base.OnDisconnectedAsync(exception);
    }
}