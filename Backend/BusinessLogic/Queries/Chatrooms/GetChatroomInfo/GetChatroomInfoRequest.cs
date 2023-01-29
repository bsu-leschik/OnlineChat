using MediatR;

namespace BusinessLogic.Queries.Chatrooms.GetChatroomInfo;

public class GetChatroomInfoRequest : IRequest<object?>
{
    public Guid ChatId { get; set; }
}