using Database.Entities;
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
        return _database.Chatrooms
                        .Include(c => c.Messages)
                        .Include(c => c.Users)
                        .AsAsyncEnumerable()
                        .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task AddChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken)
    {
        await _database.Chatrooms.AddAsync(chatroom, cancellationToken);
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
        _database.Chatrooms.Remove(chatroom);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _database.SaveChangesAsync(cancellationToken);
    }

    public IAsyncEnumerable<Chatroom> GetChatroomsAsync(CancellationToken cancellationToken)
    {
        return _database.Chatrooms
                        .Include(c => c.Users)
                        .Include(c => c.Messages)
                        .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<User> GetUsersAsync(CancellationToken cancellationToken)
    {
        return _database.Users
                        .Include(u => u.Chatrooms)
                        .AsAsyncEnumerable();
    }

    public async Task AddMessageTo(Chatroom chatroom, Message message, CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
}