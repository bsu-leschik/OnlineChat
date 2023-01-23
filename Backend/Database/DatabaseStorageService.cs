using Entities;
using Entities.Chatrooms;
using Extensions;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseStorageService : IStorageService
{
    private readonly Database _database;

    public DatabaseStorageService(Database database)
    {
        _database = database;
    }

    public Task<User?> GetUserAsync(Func<User, bool> predicate, CancellationToken cancellationToken)
    {
        return _database.Users
                        .Include(u => u.Chatrooms)
                        .AsAsyncEnumerable()
                        .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<Chatroom?> GetChatroomAsync(Func<Chatroom, bool> predicate, CancellationToken cancellationToken)
    {
        return Chatrooms()
               .AsAsyncEnumerable()
               .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task AddChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken)
    {
        await _database.AddAsync(chatroom, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await _database.Users.AddAsync(user, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(User user, CancellationToken cancellationToken)
    {
        _database.Users.Remove(user);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Chatroom chatroom, CancellationToken cancellationToken)
    {
        _database.Remove(chatroom);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _database.SaveChangesAsync(cancellationToken);
    }

    public IAsyncEnumerable<Chatroom> GetChatroomsAsync(CancellationToken cancellationToken)
    {
        return Chatrooms()
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<User> GetUsersAsync(CancellationToken cancellationToken)
    {
        return _database.Users
                        .Include(u => u.Chatrooms)
                        .AsAsyncEnumerable();
    }

    private IQueryable<Chatroom> Chatrooms()
    {
        return _database.Chatroom
                        .Include(c => c.Users)
                        .Include(c => c.Messages)
                        .Include(c => (c as PublicChatroom).Administrators)
                        .ThenInclude(c => c.Moderators);
    }
}