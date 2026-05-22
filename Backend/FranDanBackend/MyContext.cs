using FranDanBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.Mail;

namespace FranDanBackend
{
    //Context Bazy Danych, koniecznie dziedziczenie po DbContext
    //Pamiętajcie o migracji!
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Verifier> Verifiers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=FranDanDB.db");
                //Jeśli chcemy żeby zapytania SQL wyświetlały się do konsoli można dopisać .LogTo(Console.Write, LogLevel.Information)
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konwersja MailAddress <-> string dla Entity Framework
            var mailAddressConverter = new ValueConverter<MailAddress, string>(
                v => v.Address,
                v => new MailAddress(v));

            modelBuilder.Entity<User>()
                .Property(u => u.email)
                .HasConversion(mailAddressConverter);

            // Relacja Plan -> Administrator (User)
            modelBuilder.Entity<Plan>()
                .HasOne(p => p.administrator)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacje self-referential User (znajomi, zaproszenia, czarna lista)
            modelBuilder.Entity<User>()
                .HasMany(u => u.friends)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserFriends",
                    j => j.HasOne<User>().WithMany().HasForeignKey("FriendId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.friendRequests)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserFriendRequests",
                    j => j.HasOne<User>().WithMany().HasForeignKey("RequestorId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.blackList)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserBlacklist",
                    j => j.HasOne<User>().WithMany().HasForeignKey("BlockedId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));
        }
    }
}
