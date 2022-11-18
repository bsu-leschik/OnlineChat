using Microsoft.AspNetCore.SignalR;

namespace OnlineChat.Hubs;

public class ChatHub : Hub
{
    private readonly UsernameDictionary _usernames;
    private const string DefaultName = "Somebody";

    public ChatHub(UsernameDictionary usernames)
    {
        _usernames = usernames;
    }

    public async Task SendMessageFromServer(string message)
    {
        await Clients.All.SendCoreAsync("Send", new object?[] { "Server", message });
    }
    public override async Task OnConnectedAsync()
    {
        _usernames.Add(Context.ConnectionId, DefaultName);
        await SendMessageFromServer("New user connected!");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = _usernames.Get(Context.ConnectionId);
        await SendMessageFromServer($"{username} disconnected");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task Send(string name, string message)
    {
        _usernames.Set(Context.ConnectionId, name);
        await Clients.All.SendCoreAsync("Send", new object?[]{ name, message });
    }
}