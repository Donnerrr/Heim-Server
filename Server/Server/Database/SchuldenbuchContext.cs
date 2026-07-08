// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: SchuldenbuchContext.cs
//  */
using Microsoft.EntityFrameworkCore;
using Schuldenbuch.Core.Entities;

namespace Server.Database
{
    public class SchuldenbuchContext : DbContext
    {
        public SchuldenbuchContext(DbContextOptions<SchuldenbuchContext> options) : base(options)
        {    
        }


        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<DebtEntity> Debts { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Set the default schema for this Context to "schuldenbuch"
            modelBuilder.HasDefaultSchema("schuldenbuch");

            // 2. Configure the table names for each entity
            modelBuilder.Entity<PersonEntity>()
                .ToTable("Persons", "public"); // Explicitly set the table name and schema for PersonEntity

            modelBuilder.Entity<DebtEntity>()
                .ToTable("Debts", "public"); // Explicitly set the table name and schema for DebtEntity

            
            // 3. Configure the relationships between entities
            // Configure the relationship between PersonEntity and UserEntity
            modelBuilder.Entity<PersonEntity>()
                .HasOne(p => p.User)
                .WithMany(u => u.Persons)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Specify delete behavior

            // Configure the relationship between DebtEntity and PersonEntity
            modelBuilder.Entity<DebtEntity>()
                .HasOne(d => d.Person)
                .WithMany(p => p.Debts)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Specify delete behavior

        }
    }
}
