using Database.Entities;

namespace Database;

public interface IStorageService
{
    public Task<User?> GetUserAsync(Func<User, bool> predicate, CancellationToken cancellationToken);
    public Task<Chatroom?> GetChatroomAsync(Func<Chatroom, bool> predicate, CancellationToken cancellationToken);
    public Task AddChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken);
    public Task AddUserAsync(User user, CancellationToken cancellationToken);
    public Task RemoveAsync(User user, CancellationToken cancellationToken);
    public Task RemoveAsync(Chatroom chatroom, CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
    public IAsyncEnumerable<Chatroom> GetChatroomsAsync(Func<Chatroom, bool> predicate, CancellationToken cancellationToken);
}