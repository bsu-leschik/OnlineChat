using Database.Entities;

namespace BusinessLogic.Queries.Messages.GetMessages;

public class GetMessagesResponse
{
    public List<Message>? Messages { get; set; }
}