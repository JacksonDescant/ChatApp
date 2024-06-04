using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Data;

public class ChatRoomContext : DbContext
{
    public ChatRoomContext(DbContextOptions<ChatRoomContext> options) : base(options)
    {
    }
    
    public DbSet<ChatRoom> ChatRooms { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatRoom>(
            eb =>
            {
                eb.ToTable("Rooms");
                eb.HasKey(x => x.Id);
                eb.Property(b => b.Name);
                eb.Property(b => b.Banner);
            });

    }
}