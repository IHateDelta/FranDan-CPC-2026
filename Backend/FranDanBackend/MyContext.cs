using FranDanBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace F1ProjKredek
{
    //Context Bazy Danych, koniecznie dziedziczenie po DbContext
    //Pamiętajcie o migracji!
    public class MyContext : DbContext
    {
        public DbSet<Verifier> Verifiers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=FranDanDB.db");
            //Jeśli chcemy żeby zapytania SQL wyświetlały się do konsoli można dopisać .LogTo(Console.Write, LogLevel.Information)
        }
    }
}
