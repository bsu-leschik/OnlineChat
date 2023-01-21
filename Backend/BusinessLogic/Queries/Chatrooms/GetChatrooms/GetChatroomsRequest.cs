using MediatR;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public struct GetChatroomsRequest : IRequest<List<ChatroomInfo>> {}