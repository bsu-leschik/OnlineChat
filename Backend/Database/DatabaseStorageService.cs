using Entities;
using Entities.Chatrooms;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseStorageService : IStorageService
{
    private readonly ChatDatabase _chatDatabase;

    public DatabaseStorageService(ChatDatabase chatDatabase)
    {
        _chatDatabase = chatDatabase;
    }

    public Task<User?> GetUserByUsername(string username, CancellationToken cancellationToken)
    {
        return _chatDatabase.Users
                            .Where(u => u.Username == username)
                            .IncludeChatrooms()
                            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<User?> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        return _chatDatabase.Users.Where(u => u.Id == id)
                            .IncludeChatrooms()
                            .FirstOrDefaultAsync(cancellationToken);
    }

    public IQueryable<User> GetUsersById(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        return _chatDatabase.Users
                            .Where(u => ids.Any(s => s == u.Id));
    }

    public IQueryable<User> GetUsersByUsername(IEnumerable<string> names, CancellationToken cancellationToken)
    {
        return _chatDatabase.Users
                            .Where(u => names.Any(n => n == u.Username))
                            .IncludeChatrooms();
    }

    public Task<Chatroom?> GetChatroomById(Guid id, CancellationToken cancellationToken)
    {
        return _chatDatabase.Chatroom
                            .Where(c => c.Id == id)
                            .IncludeUsers()
                            .FirstOrDefaultAsync(cancellationToken);
    }

    public IQueryable<ChatroomTicket> GetUsersChatroomTickets(Guid userId, CancellationToken cancellationToken)
    {
        return _chatDatabase.ChatroomTicket
                            .Where(c => c.UserId == userId)
                            .Include(t => t.Chatroom)
                            .ThenInclude(c => c.UserTickets)
                            .ThenInclude(t => t.User);
    }

    public IQueryable<ChatroomTicket> GetChatroomUsers(Guid chatId, CancellationToken cancellationToken)
    {
        return _chatDatabase.ChatroomTicket
                            .Where(t => t.ChatroomId == chatId)
                            .Include(t => t.User);
    }

    public IQueryable<User> GetUsers()
    {
        return _chatDatabase.Users;
    }

    public IQueryable<Chatroom> GetChatrooms()
    {
        return _chatDatabase.Chatroom;
    }

    public IQueryable<ChatroomTicket> GetChatroomTickets()
    {
        return _chatDatabase.ChatroomTicket;
    }

    public IQueryable<Message> GetMessages()
    {
        return _chatDatabase.Messages;
    }

    public async Task AddChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken = default)
    {
        await _chatDatabase.Chatroom.AddAsync(chatroom, cancellationToken);
        await _chatDatabase.SaveChangesAsync(cancellationToken);
    }

    public async Task AddChatroomTicketAsync(ChatroomTicket ticket, CancellationToken cancellationToken = default)
    {
        await _chatDatabase.AddAsync(ticket, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _chatDatabase.Users.AddAsync(user, cancellationToken);
        await _chatDatabase.SaveChangesAsync(cancellationToken);
    }

    public Task RemoveUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _chatDatabase.Users.Remove(user);
        return SaveChangesAsync(cancellationToken);
    }

    public Task RemoveChatroomAsync(Chatroom chatroom, CancellationToken cancellationToken = default)
    {
        _chatDatabase.Chatroom.Remove(chatroom);
        return SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _chatDatabase.SaveChangesAsync(cancellationToken);
    }
}

file static class Extensions
{
    public static IQueryable<Chatroom> IncludeUsers(this IQueryable<Chatroom> q)
    {
        return q.Include(c => c.UserTickets)
                .ThenInclude(t => t.User);
    }

    public static IQueryable<User> IncludeChatrooms(this IQueryable<User> q)
    {
        return q.Include(u => u.ChatroomTickets)
                .ThenInclude(t => t.Chatroom);
    }
}