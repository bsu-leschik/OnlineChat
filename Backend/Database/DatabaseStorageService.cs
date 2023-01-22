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
                        .Include(u => u.PrivateChatrooms)
                        .Include(u => u.PublicChatrooms)
                        .AsAsyncEnumerable()
                        .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<Chatroom?> GetChatroomAsync(Func<Chatroom, bool> predicate, CancellationToken cancellationToken)
    {
        return Chatrooms()
               .Include(c => c.Messages)
               .Include(c => c.Users)
               .AsAsyncEnumerable()
               .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task AddChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken)
    {
        switch (chatroom)
        {
            case PublicChatroom pc:
                await _database.PublicChatrooms.AddAsync(pc, cancellationToken);
                break;
            case PrivateChatroom prc:
                await _database.PrivateChatrooms.AddAsync(prc, cancellationToken);
                break;
            default:
                return;
        }
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
        switch (chatroom)
        {
            case PublicChatroom pc:
                _database.PublicChatrooms.Remove(pc);
                break;
            case PrivateChatroom prc:
                _database.PrivateChatrooms.Remove(prc);
                break;
            default:
                return;
        }
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _database.SaveChangesAsync(cancellationToken);
    }

    public IAsyncEnumerable<Chatroom> GetChatroomsAsync(CancellationToken cancellationToken)
    {
        return Chatrooms()
               .Include(c => c.Users)
               .Include(c => c.Messages)
               .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<User> GetUsersAsync(CancellationToken cancellationToken)
    {
        return _database.Users
                        .Include(u => u.PrivateChatrooms)
                        .Include(u => u.PublicChatrooms)
                        .AsAsyncEnumerable();
    }

    private IQueryable<Chatroom> Chatrooms()
    {
        return _database.PrivateChatrooms.Cast<Chatroom>().Union(_database.PublicChatrooms);
    }
}