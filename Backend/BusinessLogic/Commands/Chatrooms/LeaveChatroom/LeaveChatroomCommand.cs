using MediatR;

namespace BusinessLogic.Commands.Chatrooms.LeaveChatroom;

public class LeaveChatroomCommand : IRequest<LeaveChatroomResponse>
{
    public Guid ChatId { get; set; }
}