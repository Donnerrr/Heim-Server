using System;
using System.Collections.Generic;
using System.Text;
using Schuldenbuch.Core.DTOs;
using Schuldenbuch.Core.Interfaces;
using Schuldenbuch.Core.DTOs.DebtDtos;

namespace Schuldenbuch.Core.Services
{
    public class DebtService : IDebtService
    {
        private readonly ISchuldenbuchDatabase _db;

        public DebtService(ISchuldenbuchDatabase db)
        {
            _db = db;
        }


        public async Task<AddDebtStatusDto> AddDebtAsync(AddDebtDto dto)
        {
            var person = await _db.GetPersonAsync(dto.PersonId);

            if (person == null)
            {
                return new AddDebtStatusDto
                {
                    Status = DebtStatus.IdNotFound,
                    Message = $"Keine Person mit ID {dto.PersonId} gefunden."
                };
            }

            var debt = new Entities.DebtEntity
            {
                PersonId = dto.PersonId,
                Amount = dto.Amount,
                Reason = dto.Description,
                Date = DateTime.UtcNow,
                PaidDate = null
            };

            await _db.AddDebtAsync(debt);

            return new AddDebtStatusDto()
            {
                Status = DebtStatus.Success,
                Message = $"Schulden mit ID {debt.Id} gespeichert."
            };
        }


        public async Task<DeleteDebtResultDto> DeleteDebtAsync(int id)
        {
            var Debt = await _db.GetDebtAsync(id);

            if (Debt == null)
            {
                return new DeleteDebtResultDto()
                {
                    Status = DTOs.DebtDtos.DeleteStatus.NotFound,
                    Message = $"Keine Schuld mit der Id {id} in der Datenbank"
                };
            }

            await _db.DeleteDebtAsync(id);

            return new DeleteDebtResultDto()
            {
                Status = DTOs.DebtDtos.DeleteStatus.Success,
                Message = "Löschung erfolgreich"
            };

        }


        public async Task<UpdateDebtResultDto> UpdateDebtAsync(int id, decimal amount, bool isAddition)
        {
            var debt = await _db.GetDebtAsync(id);

            if (debt == null)
            {
                // Abfangen, statt abstürzen!
                return new UpdateDebtResultDto
                {
                    Status = UpdateStatus.Failed,
                    Message = "Die zu aktualisierende Schuld wurde in der Datenbank nicht gefunden."
                };
            }

            decimal dif = isAddition ? amount : -amount;

            decimal newAmount = debt.Amount + dif;

            if (newAmount <= 0)
            {
                
                await DeleteDebtAsync(id);

                return new UpdateDebtResultDto()
                {
                    Id = id,
                    Amount = 0,
                    Status = UpdateStatus.Success,
                    Message = "Der Eintrag wurde abbezahlt."
                };
            }

            else
            {
                debt.Amount = newAmount;
                await _db.UpdateDebtAsync(debt);
                return new UpdateDebtResultDto()
                {
                    Id = id,
                    Amount = debt.Amount,
                    Status = UpdateStatus.Success,
                    Message = $"Schuld mit ID {id} erfolgreich aktualisiert."
                };
            }


        }
    }
}