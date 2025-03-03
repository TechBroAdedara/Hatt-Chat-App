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
}
