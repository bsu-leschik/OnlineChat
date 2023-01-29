namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class GetChatroomsResult
{
    public GetChatroomsResult(List<object> chatrooms)
    {
        Chatrooms = chatrooms;
    }

    public List<object> Chatrooms { get; set; }
}