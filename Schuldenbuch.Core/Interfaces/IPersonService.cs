// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: IPersonService.cs
//  */

using Schuldenbuch.Core.DTOs.PersonDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schuldenbuch.Core.Interfaces
{
    public interface IPersonService
    {
        Task<AddPersonResultDto> AddPersonAsync(AddPersonDto dto);

        Task<DeletePersonResultDto> DeletePersonAsync(int id);

        Task<GetPersonResultDto?> GetPersonAsync(int id);

        Task<List<PersonListItemDto>> GetAllPersonsAsync(int userID);
    }
}
