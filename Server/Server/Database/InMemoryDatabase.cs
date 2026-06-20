// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: InMemoryDatabase.cs
//  */


using Microsoft.EntityFrameworkCore;
using Schuldenbuch.Core.DTOs.DebtDtos;
using Schuldenbuch.Core.Entities;
using Schuldenbuch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Schuldenbuch.Core.Database
{
    public class Database : ISchuldenbuchDatabase
    {
        private readonly List<PersonEntity> _persons;   // Liste zur Speicherung von Personen
        private readonly List<DebtEntity> _debts;       // Liste zur Speicherung von Schulden

        private int _personIdCounter;                   // Zähler für die Vergabe von eindeutigen IDs für Personen
        private int _debtIdCounter;                     // Zähler für die Vergabe von eindeutigen IDs für Schulden


        public Database()                               // Konstruktor, der die Listen und Zähler initialisiert
        {
            _persons = new List<PersonEntity>();
            _debts = new List<DebtEntity>();

            _personIdCounter = 1;
            _debtIdCounter = 1;
        }

        public Task  AddPersonAsync(PersonEntity person)      // Methode zum Hinzufügen einer Person zur Datenbank
        {
            person.Id = _personIdCounter;
            _personIdCounter++;
            _persons.Add(person);
            return Task.CompletedTask;
        }

        public Task AddDebtAsync(DebtEntity debt)            // Methode zum Hinzufügen einer Schuld zur Datenbank
        {
            debt.Id = _debtIdCounter;
            _debtIdCounter++;
            _debts.Add(debt);
            return Task.CompletedTask;
        }

        public Task<PersonEntity?> GetPersonAsync(int id)
        {
            var persons =  _persons.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(persons);
        }

        public Task<List<DebtEntity>> GetDebtsForPersonAsync(int personId)
        {
            var debts =  _debts.Where(d => d.PersonId == personId).ToList();
            return Task.FromResult(debts);
        }

        public Task DeletePersonAsync(int id)
        {
            var person = _persons.FirstOrDefault(p => p.Id == id);
            if (person != null)
            {
                _persons.Remove(person);
                _debts.RemoveAll(d => d.PersonId == id);
            }
            return Task.CompletedTask;
        }

        public Task DeleteDebtAsync(int id)
        {
            var debt = _debts.FirstOrDefault(d => d.Id == id);
            if (debt != null)
            {
                _debts.Remove(debt);
            }
            return Task.CompletedTask;
        }

        public Task<List<PersonEntity>> GetAllPersonsAsync()
        {
            return Task.FromResult(_persons.ToList());
        }

        public Task<DebtEntity?> GetDebtAsync(int id)
        {
            var debt = _debts.FirstOrDefault(p => p.Id == id);

            return Task.FromResult(debt);
        }

        public async Task UpdateDebtAsync(DebtEntity debt)          // Methode zum Aktualisieren einer Schuld in der Datenbank
        {
           
        }
    }
}
