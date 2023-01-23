using Entities;
using Entities.Chatrooms;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class Database : DbContext
{
    public Database(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PublicChatroom>()
                    .HasOne(c => c.Administrators)
                    .WithOne(a => a.PublicChatroom)
                    .HasForeignKey<Administrators>(a => a.PublicChatroomId);

        modelBuilder.Entity<Administrators>()
                    .HasOne(a => a.Owner)
                    .WithMany();
        
        modelBuilder.Entity<Administrators>()
                    .HasMany(a => a.Moderators)
                    .WithMany();

        modelBuilder.Entity<PublicChatroom>()
                    .HasBaseType<Chatroom>();

        modelBuilder.Entity<PrivateChatroom>()
                    .HasBaseType<Chatroom>();

        modelBuilder.Entity<Chatroom>()
                    .HasMany(c => c.Messages);
        modelBuilder.Entity<Chatroom>()
                    .HasMany(c => c.Users)
                    .WithMany(u => u.Chatrooms);
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Chatroom> Chatroom { get; set; } = null!;
    public DbSet<PublicChatroom> PublicChatroom { get; set; } = null!;
    public DbSet<PrivateChatroom> PrivateChatroom { get; set; } = null!;
}