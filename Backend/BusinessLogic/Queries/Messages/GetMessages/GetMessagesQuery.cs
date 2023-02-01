using MediatR;

namespace BusinessLogic.Queries.Messages.GetMessages;

public class GetMessagesQuery : IRequest<GetMessagesResponse>
{
    public Guid ChatId { get; set; }
    public int Offset { get; set; } = 0;
    public int Count { get; set; } = 100;
}