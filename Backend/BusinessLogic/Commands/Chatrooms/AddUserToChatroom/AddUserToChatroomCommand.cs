using MediatR;

namespace BusinessLogic.Commands.Chatrooms.AddUserToChatroom;

public class AddUserToChatroomCommand : IRequest<AddUserToChatroomResponse>
{
    public string Username { get; set; }
    public Guid ChatId { get; set; }
}