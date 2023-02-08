using Database.Migrations;
using Entities;
using Entities.Chatrooms;
using Microsoft.EntityFrameworkCore;

namespace Database;

public interface IQueryOptions<T>
{
    public IQueryable<T> ApplyTo(IQueryable<T> queryable);
    public static IQueryOptions<T> Default => new DefaultQueryOptions<T>();
}

public class DefaultQueryOptions<T> : IQueryOptions<T>
{
    public IQueryable<T> ApplyTo(IQueryable<T> queryable)
    {
        return queryable;
    }
}

public record UserQueryOptions(
    bool IncludeChatrooms = false,
    bool IncludeMessages = false,
    bool IncludeModerators = false,
    int MessagesOffset = 0,
    int MessagesLimit = 100) : IQueryOptions<User>
{
    public IQueryable<User> ApplyTo(IQueryable<User> queryable)
    {
        if (!IncludeChatrooms)
        {
            return queryable;
        }

        var q = queryable
                .Include(u => u.ChatroomTickets)
                .ThenInclude(t => t.Chatroom)
                .ThenInclude(c => (c as PublicChatroom).Administrators);
        if (IncludeModerators)
        {
            queryable = q.ThenInclude(a => a.Moderators);
        }
        else
        {
            queryable = q;
        }
        if (IncludeMessages)
        {
            queryable = queryable
                        .Include(u => u.ChatroomTickets)
                        .ThenInclude(t => t.Chatroom)
                        .ThenInclude(c => c.Messages.Skip(MessagesOffset).Take(MessagesLimit));
        }
        return queryable;
    }
}

public record ChatroomQueryOptions(
    bool IncludeMessages = false,
    int MessagesOffset = 0,
    int MessagesLimit = 100,
    bool IncludeUsers = false
) : IQueryOptions<Chatroom>
{
    public IQueryable<Chatroom> ApplyTo(IQueryable<Chatroom> queryable)
    {
        if (IncludeMessages)
        {
            queryable = queryable.Include(c => c.Messages.Skip(MessagesOffset).Take(MessagesLimit));
        }
        if (IncludeUsers)
        {
            queryable = queryable.Include(c => c.UserTickets)
                                 .ThenInclude(t => t.User);
        }
        return queryable;
    }
}

public record ChatroomTicketQueryOptions(
    bool IncludeUsers = false,
    bool IncludeChatrooms = false,
    bool IncludeMessages = false,
    int MessagesOffset = 0,
    int MessagesLimit = 100
) : IQueryOptions<ChatroomTicket>
{
    public IQueryable<ChatroomTicket> ApplyTo(IQueryable<ChatroomTicket> queryable)
    {
        if (IncludeUsers)
        {
            queryable = queryable.Include(t => t.User);
        }
        if (!IncludeChatrooms)
        {
            return queryable;
        }

        queryable = queryable
                    .Include(t => t.Chatroom)
                    .ThenInclude(c => (c as PublicChatroom).Administrators)
                    .ThenInclude(a => a.Moderators);

        if (IncludeMessages)
        {
            queryable = queryable
                        .Include(t => t.Chatroom)
                        .ThenInclude(c => c.Messages.Skip(MessagesOffset).Take(MessagesLimit));
        }
        return queryable;
    }
}