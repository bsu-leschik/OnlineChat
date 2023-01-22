using MediatR;

namespace BusinessLogic.Commands.Chatrooms.KickUserFromChatroom;

public class KickUserFromChatroomCommand : IRequest<KickUserFromChatroomResponse>
{
    public string Username { get; set; }
    public Guid ChatId { get; set; }
}