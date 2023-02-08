using Database.Migrations;
using Entities;
using Entities.Chatrooms;

namespace Database;

public interface IStorageService
{
    public Task<User?> GetUserByUsername(string username, CancellationToken cancellationToken);
    public Task<User?> GetUserById(Guid id, CancellationToken cancellationToken);
    public IQueryable<User> GetUsersById(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    public IQueryable<User> GetUsersByUsername(IEnumerable<string> names, CancellationToken cancellationToken);
    public Task<Chatroom?> GetChatroomById(Guid id, CancellationToken cancellationToken);
    public IQueryable<ChatroomTicket> GetUsersChatroomTickets(Guid userId, CancellationToken cancellationToken);
    public IQueryable<ChatroomTicket> GetChatroomUsers(Guid chatId, CancellationToken cancellationToken);
    public Task<Chatroom?> GetChatroomWithMessages(Guid chatId, CancellationToken cancellationToken, int offset = 0, int count = 100);
    public IQueryable<User> GetUsers();

    public Task AddChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken = default);

    public Task AddUserAsync(User user, CancellationToken cancellationToken = default);
    public Task RemoveUserAsync(User user, CancellationToken cancellationToken = default);
    public Task RemoveChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken = default);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}