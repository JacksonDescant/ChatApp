using Microsoft.EntityFrameworkCore;
using ChatApp.Models;

namespace ChatApp.Server.Data;

public class UserInfoContext : DbContext
{
public UserInfoContext(DbContextOptions<UserInfoContext> options) : base(options)
    {
    }

    public DbSet<UserInfo> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfo>(
            eb =>
            {
                eb.ToTable("UserInfo");
                eb.HasKey(x => x.id);
                eb.Property(b => b.Username);
                eb.Property(b => b.Password);
                eb.Property(b => b.Email);
            });

    }
}