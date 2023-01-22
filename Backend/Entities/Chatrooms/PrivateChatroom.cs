namespace Entities.Chatrooms;

public class PrivateChatroom : Chatroom
{
    public PrivateChatroom(Guid id, User a, User b) : base(id, ChatType.Private, new List<User> { a, b }) {}

    public PrivateChatroom(Guid id, List<User> users) : base(id, ChatType.Private, users)
    {
        if (users.Count != 2)
        {
            throw new ArgumentException("Private chat can be created for 2 users");
        } 
    }

    public PrivateChatroom() {}
}