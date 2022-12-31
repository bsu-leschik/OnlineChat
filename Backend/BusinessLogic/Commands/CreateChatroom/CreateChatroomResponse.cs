namespace BusinessLogic.Commands.CreateChatroom;

public class CreateChatroomResponse
{
    public CreateChatroomResponse(Guid chatId, bool created = true)
    {
        ChatId = chatId;
        Created = created;
    }

    public Guid ChatId { get; set; }
    public bool Created { get; set; }
}