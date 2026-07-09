/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 09.07.2026 21:16:05
 * Projekt: Server.Database
 * Datei: SchuldenbuchContextFactory
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Server.Database
{
    // Wird NUR von "dotnet ef" (migrations add / database update) zur Design-Time genutzt.
    // Beim normalen App-Start (Program.cs) greift diese Klasse NICHT.
    // Dadurch kann "dotnet ef" nie mehr versehentlich gegen Prod laufen,
    // egal was appsettings.json, appsettings.Production.json oder
    // System-Umgebungsvariablen aktuell sagen.
    public class SchuldenbuchContextFactory : IDesignTimeDbContextFactory<SchuldenbuchContext>
    {
        public SchuldenbuchContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<SchuldenbuchContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new SchuldenbuchContext(optionsBuilder.Options);
        }
    }
}
