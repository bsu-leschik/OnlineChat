using System.ComponentModel.DataAnnotations.Schema;
using Entities.Chatrooms;

namespace Entities;

public class ChatroomTicket
{
    public Guid UserId { get; set; }
    public Guid ChatroomId { get; set; }
    public User User { get; set; }
    public Chatroom Chatroom { get; set; }
    public int LastMessageRead { get; set; }

    public ChatroomTicket() {} // for EF Core

    public ChatroomTicket(User user, Chatroom chatroom)
    {
        User = user;
        Chatroom = chatroom;
        LastMessageRead = chatroom.Messages.Count;
        UserId = user.Id;
        ChatroomId = chatroom.Id;
    }
}