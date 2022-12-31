using MediatR;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class GetChatroomsRequest : IRequest<List<ChatroomInfo>> {}