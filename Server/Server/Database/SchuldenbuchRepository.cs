/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 20.06.2026 02:25:13
 * Projekt: Server.Database
 * Datei: PostgreDatabase
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

using Schuldenbuch.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Schuldenbuch.Core.Interfaces;

namespace Server.Database
{
    /// <summary>
    /// /////////////////////////////////////////
    /// </summary>
    public class SchuldenbuchRepository : ISchuldenbuchDatabase
    {
        private readonly SchuldenbuchContext _context;

        public SchuldenbuchRepository(SchuldenbuchContext context)
        {
            _context = context;
        }

        public async Task AddPersonAsync(PersonEntity person)      // Methode zum Hinzufügen einer Person zur Datenbank
        {
            await _context.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task AddDebtAsync(DebtEntity debt)            // Methode zum Hinzufügen einer Schuld zur Datenbank
        {
            await _context.AddAsync(debt);
            await _context.SaveChangesAsync();
        }

        public async Task<PersonEntity?> GetPersonAsync(int id)           // Methode zum Abrufen einer Person anhand ihrer ID aus der Datenbank
        {

            return await _context.Persons.FindAsync(id);
        }

        public async Task<List<DebtEntity>> GetDebtsForPersonAsync(int personId)                 // Methode zum Abrufen aller Schulden einer Person anhand ihrer ID aus der Datenbank
        {
            return await _context.Debts.Where(d => d.PersonId == personId).ToListAsync();
        }

        public async Task DeletePersonAsync(int id)                // Methode zum Löschen einer Person und aller zugehörigen Schulden aus der Datenbank anhand ihrer ID
        {
            var person = await GetPersonAsync(id);
            if (person != null)
            {
                var debs = _context.Debts.Where(d => d.PersonId == id);
                _context.Debts.RemoveRange(debs);

                _context.Remove(person);

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteDebtAsync(int id)          // Methode zum Löschen einer Schuld aus der Datenbank anhand ihrer ID
        {
            var debt = _context.Debts.Find(id);
            if (debt != null)
            {
                _context.Remove(debt);

                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<PersonEntity>> GetAllPersonsAsync()           // Methode zum Abrufen aller Personen aus der Datenbank
        {
            return await _context.Persons.ToListAsync();
        }


        public async Task<DebtEntity?> GetDebtAsync(int id)                   // Methode zum Abrufen einer Schuld anhand ihrer ID aus der Datenbank
        {
            return await _context.Debts.FindAsync(id);
        }


        public async Task UpdateDebtAsync(DebtEntity debt)          // Methode zum Aktualisieren einer Schuld in der Datenbank
        {
            var existingDebt = await _context.Debts.FindAsync(debt.Id);
            if (existingDebt != null)
            {
                existingDebt.Amount = debt.Amount;
                existingDebt.Date = debt.Date;
                await _context.SaveChangesAsync();
            }


        }

        public async Task<UserEntity?> GetUserByUsernameAsync(string username) // Methode zum Abrufen des Users anhand des Usernamens
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task AddUserAsync(UserEntity user) //Methode zum Hinzufügen eines Users
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
