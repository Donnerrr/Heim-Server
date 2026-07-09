using Schuldenbuch.Core.DTOs.DebtDtos;
using Schuldenbuch.Core.DTOs.PersonDtos;
using Schuldenbuch.Core.Entities;


namespace Schuldenbuch.Core.Interfaces
{
    public interface ISchuldenbuchDatabase
    {
        // Personen-Operationen
        Task AddPersonAsync(PersonEntity person);
        Task<PersonEntity?> GetPersonAsync(int id);
        Task<List<PersonEntity>> GetAllPersonsAsync();
        Task DeletePersonAsync(int personId);


        // Schulden-Operationen
        Task AddDebtAsync(DebtEntity debt);
        Task<List<DebtEntity>> GetDebtsForPersonAsync(int personId);
        Task<DebtEntity?> GetDebtAsync(int id);
        Task DeleteDebtAsync(int id);

        Task UpdateDebtAsync(DebtEntity debt);

        // User-Operationen
        Task<UserEntity> GetUserByUsernameAsync(string userName);
        Task AddUserAsync(UserEntity user);
    }
}