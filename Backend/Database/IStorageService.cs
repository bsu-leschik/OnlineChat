using Database.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database;

public interface IStorageService
{
    public Task<User?> GetUser(Func<User, bool> predicate, CancellationToken cancellationToken);
    public Task<Chatroom?> GetChatroom(Func<Chatroom, bool> predicate, CancellationToken cancellationToken);
    public Task AddChatroom(Chatroom chatroom, CancellationToken cancellationToken);
    public Task AddUser(User user, CancellationToken cancellationToken);
    public Task Remove(User user, CancellationToken cancellationToken);
    public Task Remove(Chatroom chatroom, CancellationToken cancellationToken);
    public Task<List<Chatroom>> GetChatrooms(CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
    public Task AddMessageTo(Chatroom chatroom, Message message, CancellationToken cancellationToken);
}