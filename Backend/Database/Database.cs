using Entities;
using Entities.Chatrooms;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class Database : DbContext
{
    public Database(DbContextOptions options) : base(options) {}

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<PublicChatroom> PublicChatrooms { get; set; } = null!;
    public DbSet<PrivateChatroom> PrivateChatrooms { get; set; } = null!;
}