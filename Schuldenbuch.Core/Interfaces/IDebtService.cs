// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: IDebtService.cs
//  */

/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 12.06.2026 22:51:09
 * Projekt: Schuldenbuch.Core.Interfaces
 * Datei: IDebtService
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

using Schuldenbuch.Core.DTOs.DebtDtos;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Schuldenbuch.Core.Interfaces
{
    /// <summary>
    /// /////////////////////////////////////////
    /// </summary>
    public interface IDebtService
    {
        Task<AddDebtStatusDto> AddDebtAsync(AddDebtDto dto);

        Task<DeleteDebtResultDto> DeleteDebtAsync(int id);

        Task<UpdateDebtResultDto> UpdateDebtAsync(int id, string amount);
    }
}