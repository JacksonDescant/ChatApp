using Microsoft.EntityFrameworkCore;
using ChatApp.Models;

namespace ChatApp.Server.Data;

public class MessageLogsContext : DbContext
{
    public MessageLogsContext(DbContextOptions<MessageLogsContext> options) : base(options)
    {
    }
    
    public DbSet<MessageLogs> MessageLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageLogs>(
            eb =>
            {
                eb.ToTable("MessageLogs");
                eb.HasKey(x => x.Id);
                eb.Property(b => b.Username);
                eb.Property(b => b.Message);
                eb.Property(b => b.Timestamp);
            });

    }
}