using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class Database : DbContext
{
    public Database(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Chatroom> Chatrooms { get; set; }
}