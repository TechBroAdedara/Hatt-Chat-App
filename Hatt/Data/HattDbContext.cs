using System;
using Hatt.Models;
using Microsoft.EntityFrameworkCore;

namespace Hatt.Data;

public class HattDbContext(DbContextOptions<HattDbContext> options) : DbContext(options)
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationUser> ConversationsUsers { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ensure UserName is unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        // Ensure Email is unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Ensure PhoneNumber is unique (if it's not nullable)
        modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();
        modelBuilder.Entity<FriendRequest>()
            .HasOne(fr => fr.Sender)
            .WithMany(u => u.SentFriendRequests)
            .HasForeignKey(fr => fr.SenderId)
            .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete issues

        modelBuilder.Entity<FriendRequest>()
            .HasOne(fr => fr.Reciever)
            .WithMany(u => u.RecievedFriendRequests)
            .HasForeignKey(fr => fr.RecieverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Conversation>()
           .Property(c => c.Id)
           .HasColumnType("CHAR(36)")
           .HasDefaultValueSql("(UUID())"); // Generates GUID in MySQL

        modelBuilder.Entity<Message>()
            .Property(m => m.ConversationId)
            .HasColumnType("CHAR(36)");

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
