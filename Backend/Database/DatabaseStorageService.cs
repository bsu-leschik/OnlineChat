using Entities;
using Entities.Chatrooms;
using Extensions;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseStorageService : IStorageService
{
    private readonly ChatDatabase _chatDatabase;

    public DatabaseStorageService(ChatDatabase chatDatabase)
    {
        _chatDatabase = chatDatabase;
    }

    public Task<User?> GetUserAsync(Func<User, bool> predicate, CancellationToken cancellationToken)
    {
        return _chatDatabase.Users
                        .Include(u => u.ChatroomTickets)
                        .ThenInclude(t => t.Chatroom)
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
        await _chatDatabase.AddAsync(chatroom, cancellationToken);
        await _chatDatabase.SaveChangesAsync(cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await _chatDatabase.Users.AddAsync(user, cancellationToken);
        await _chatDatabase.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(User user, CancellationToken cancellationToken)
    {
        _chatDatabase.Users.Remove(user);
        await _chatDatabase.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Chatroom chatroom, CancellationToken cancellationToken)
    {
        _chatDatabase.Remove(chatroom);
        await _chatDatabase.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _chatDatabase.SaveChangesAsync(cancellationToken);
    }

    public IAsyncEnumerable<Chatroom> GetChatroomsAsync(CancellationToken cancellationToken)
    {
        return Chatrooms().AsAsyncEnumerable();
    }

    public IAsyncEnumerable<User> GetUsersAsync(CancellationToken cancellationToken)
    {
        return _chatDatabase.Users
                        .Include(u => u.ChatroomTickets)
                        .ThenInclude(t => t.Chatroom)
                        .AsAsyncEnumerable();
    }

    private IQueryable<Chatroom> Chatrooms()
    {
        return _chatDatabase.Chatroom
                        .Include(c => c.Users)
                        .ThenInclude(u => u.ChatroomTickets)
                        .Include(c => c.Messages)
                        .Include(c => (c as PublicChatroom)!.Administrators)
                        .ThenInclude(c => c.Moderators);
    }
}