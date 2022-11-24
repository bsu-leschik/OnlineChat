using OnlineChat.Models;

namespace OnlineChat.Hubs.SendMessageApprover;

public interface ISendMessageVerifier
{
    public bool Approve(User sender, string message);
}