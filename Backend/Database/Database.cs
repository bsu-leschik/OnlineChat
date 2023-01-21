using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class Database : DbContext
{
    public Database(DbContextOptions options) : base(options) {}

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Chatroom> Chatrooms { get; set; } = null!;
}