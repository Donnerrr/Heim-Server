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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Set the default schema for this Context to "schuldenbuch"
            modelBuilder.HasDefaultSchema("schuldenbuch");


            modelBuilder.Entity<PersonEntity>()
                .ToTable("Persons", "public"); // Explicitly set the table name and schema for PersonEntity

            modelBuilder.Entity<DebtEntity>()
                .ToTable("Debts", "public"); // Explicitly set the table name and schema for DebtEntity

        }
    }
}
