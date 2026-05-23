using FranDanBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.Mail;

namespace FranDanBackend
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Verifier> Verifiers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=FranDanDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            var mailAddressConverter = new ValueConverter<MailAddress, string>(
                v => v.Address,
                v => new MailAddress(v));

            modelBuilder.Entity<User>()
                .Property(u => u.email)
                .HasConversion(mailAddressConverter);

            modelBuilder.Entity<Plan>()
                .HasOne(p => p.administrator)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.friend1)
                .WithMany()
                .HasForeignKey(f => f.friend1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.friend2)
                .WithMany()
                .HasForeignKey(f => f.friend2Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
