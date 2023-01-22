using Database.Entities;
using MediatR;

namespace BusinessLogic.Commands.Chatrooms.CreateChatroom;

public class CreateChatroomCommand : IRequest<CreateChatroomResponse>
{
    public Chatroom.ChatType Type { get; set; }
    public List<string> Usernames { get; set; }
}