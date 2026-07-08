// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: PersonService.cs
//  */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Schuldenbuch.Core.DTOs;
using Schuldenbuch.Core.Interfaces;
using Schuldenbuch.Core.DTOs.PersonDtos;
using Schuldenbuch.Core.DTOs.DebtDtos;



namespace Schuldenbuch.Core.Services
{
    public class PersonService : IPersonService
    {
        private readonly ISchuldenbuchDatabase _db;

        public PersonService(ISchuldenbuchDatabase db)
        {
            _db = db;
        }


        public async Task<AddPersonResultDto> AddPersonAsync(AddPersonDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Name))
            {
                return new AddPersonResultDto()
                {
                    Status = AddPersonStatus.ValidationError,
                    Message = "Name darf nicht leer sein."
                };
            } 

            if(string.IsNullOrWhiteSpace(dto.Street))
            {
                return new AddPersonResultDto()
                {
                    Status = AddPersonStatus.ValidationError,
                    Message = "Straße darf nicht leer sein."
                };
            }

            if(string.IsNullOrWhiteSpace(dto.ZipCode))
            {
                return new AddPersonResultDto()
                {
                    Status = AddPersonStatus.ValidationError,
                    Message = "PLZ darf nicht leer sein."
                };
            }

            if(string.IsNullOrWhiteSpace(dto.City))
            {
                return new AddPersonResultDto()
                {
                    Status = AddPersonStatus.ValidationError,
                    Message = "Ort darf nicht leer sein."
                };
            }



            var person = new Entities.PersonEntity
            {
                Name = dto.Name,
                Street = dto.Street,
                ZipCode = dto.ZipCode,
                City = dto.City
            };

            await _db.AddPersonAsync(person);

            return new AddPersonResultDto()
            {
                Status = AddPersonStatus.Success,
                Id = person.Id,
                Message = $"Person erfolgreich mit der ID {person.Id} gespeichert."
            };
        }

        public async Task<DeletePersonResultDto> DeletePersonAsync(int id)
        {

            var person = await _db.GetPersonAsync(id);

            if (person == null) 
            {
                return new DeletePersonResultDto()
                {
                    Status = DTOs.PersonDtos.DeleteStatus.NotFound,
                    Message = $"Person mit ID {id} nicht in der Datenbank gefunden."
                };
            }

            await _db.DeletePersonAsync(id);

            return new DeletePersonResultDto()
            {
                Status = DTOs.PersonDtos.DeleteStatus.Success,
                Message = $"Person {person.Name} gelöscht."
            };

        }

        public async Task<GetPersonResultDto?> GetPersonAsync(int id)
        {

            var Entity = await _db.GetPersonAsync(id);

            if (Entity == null)
            {
                return new GetPersonResultDto
                {
                    Status = GetPersonStatus.NotFound,
                    Message = $"Person mit ID {id} nicht in der Datenbank gefunden.",
                    
                };
            }

            var debts = await _db.GetDebtsForPersonAsync(id);

            decimal amount = debts.Sum(d => d.Amount);

            var dto = new GetPersonDto()
            {
                Name = Entity.Name,
                Street = Entity.Street,
                ZipCode = Entity.ZipCode,
                City = Entity.City,
                Amount = amount,
                Debts = debts.Select(d => new DebtListItemDto()
                {
                    Id = d.Id,
                    PersonId = d.PersonId,
                    Amount = d.Amount,
                    Reason = d.Reason,
                    Date = d.Date
                }).ToList()
            };

            return new GetPersonResultDto()
            {
                Status = GetPersonStatus.Success,
                Message = "Gefunden",
                Person = dto
            };

        }

        public async Task<List<PersonListItemDto>> GetAllPersonsAsync()
        {
            var entities = await _db.GetAllPersonsAsync();

            return entities.Select(p => new PersonListItemDto
            {
                Id = p.Id,
                Name = p.Name,
                Street = p.Street,
                ZipCode = p.ZipCode,
                City = p.City
            }).ToList();
        }




    }
}
