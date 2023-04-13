using ChatApp.Entities;
using Microsoft.EntityFrameworkCore;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<GroupChat> GroupChats { get; set; }
    public DbSet<IndividualChat> IndividualChats { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.; database=ChatApp2; Integrated security=true; TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroupChat>()
            .HasOne(p => p.Creator)
            .WithMany()
            .HasForeignKey(p => p.CreatorId);

        modelBuilder.Entity<IndividualChat>()
            .HasOne(p => p.UserOne)
            .WithMany()
            .HasForeignKey(p => p.UserOneId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        modelBuilder.Entity<IndividualChat>()
            .HasOne(p => p.UserTwo)
            .WithMany()
            .HasForeignKey(p => p.UserTwoId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        modelBuilder.Entity<Chat>()
            .HasOne(p => p.Sender)
            .WithMany()
            .HasForeignKey(p => p.SenderId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        base.OnModelCreating(modelBuilder);
    }
}
