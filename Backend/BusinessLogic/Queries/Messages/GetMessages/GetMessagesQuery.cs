using MediatR;

namespace BusinessLogic.Queries.Messages.GetMessages;

public class GetMessagesQuery : IRequest<GetMessagesResponse>
{
    public Guid ChatId { get; set; }
}