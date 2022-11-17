using Microsoft.AspNetCore.SignalR;

namespace OnlineChat.Hubs;

public class ChatHub : Hub
{
    public async Task Send(string name, string message)
    {
        await Clients.All.SendCoreAsync("Send", new object?[]{ name, message });
    }
}