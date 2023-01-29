using Entities;

namespace BusinessLogic.Hubs.Chat;

public interface IChatClientInterface
{
    public Task Receive(Message message, string chatId);
    public Task PromoteToTop(string chatId);
}