using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseStorageService : IStorageService
{
    private readonly Database _database;

    public DatabaseStorageService(Database database)
    {
        _database = database;
    }
    
    public Task<User?> GetUser(Func<User, bool> predicate, CancellationToken cancellationToken)
    {
        return _database.Users.FindAsync(predicate, cancellationToken).AsTask();
    }

    public Task<Chatroom?> GetChatroom(Func<Chatroom, bool> predicate, CancellationToken cancellationToken)
    {
        return _database.Chatrooms.FindAsync(predicate, cancellationToken).AsTask();
    }

    public async Task AddChatroom(Chatroom chatroom, CancellationToken cancellationToken)
    {
        await _database.Chatrooms.AddAsync(chatroom, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task AddUser(User user, CancellationToken cancellationToken)
    {
        await _database.Users.AddAsync(user, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task Remove(User user, CancellationToken cancellationToken)
    {
        _database.Users.Remove(user);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task Remove(Chatroom chatroom, CancellationToken cancellationToken)
    {
        _database.Chatrooms.Remove(chatroom);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public Task<List<Chatroom>> GetChatrooms(CancellationToken cancellationToken)
    {
        return _database.Chatrooms.ToListAsync(cancellationToken);
    }
}