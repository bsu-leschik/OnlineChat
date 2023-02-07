using Entities;
using Entities.Chatrooms;

namespace Database;

public interface IStorageService
{
    public IQueryable<User> GetUsers();
    public IQueryable<Chatroom> GetChatrooms();
    public IQueryable<ChatroomTicket> GetChatroomTickets();
    public IQueryable<Message> GetMessages();
    public Task AddChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken = default);
    public Task AddChatroomTicketAsync(ChatroomTicket ticket, CancellationToken cancellationToken = default);

    public Task AddUserAsync(User user, CancellationToken cancellationToken = default);
    public Task RemoveUserAsync(User user, CancellationToken cancellationToken = default);
    public Task RemoveChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken = default);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}