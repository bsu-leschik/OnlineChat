using OnlineChat.Models;

namespace OnlineChat.Hubs.SendMessageApprover;

public class SendMessageGuard
{
    private readonly List<ISendMessageVerifier> _verifiers = new();

    public SendMessageGuard AddVerifier(ISendMessageVerifier verifier)
    {
        _verifiers.Add(verifier);
        return this;
    }
    
    public bool Approve(User user, string message)
    {
        return _verifiers.All(v => v.Approve(user, message));
    }
}

public class EmptyOrWhitespaceMessageVerifier : ISendMessageVerifier
{
    public bool Approve(User sender, string message)
    {
        return !string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message);
    }
}

public class IsSpamVerifier : ISendMessageVerifier
{
    public bool Approve(User sender, string message)
    {
        return DateTime.Now - sender.LastMessageTime > Constants.AntispamMinTime;
    }
}