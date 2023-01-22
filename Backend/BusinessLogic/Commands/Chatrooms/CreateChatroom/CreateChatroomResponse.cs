namespace BusinessLogic.Commands.Chatrooms.CreateChatroom;

public class CreateChatroomResponse
{
    public CreateChatroomResponse(Guid chatId, bool created = true)
    {
        ChatId = chatId;
        Created = created;
    }

    public Guid ChatId { get; set; }
    public bool Created { get; set; }

    public static CreateChatroomResponse Failed => new CreateChatroomResponse(
        chatId: Guid.Empty,
        created: false
    );
}