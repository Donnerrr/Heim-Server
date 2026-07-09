// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: IAuthService.cs
//  */

/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 09.07.2026 21:23:45
 * Projekt: Schuldenbuch.Core.Interfaces
 * Datei: IAuthService
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

using Schuldenbuch.Core.Entities;
using System;

namespace Schuldenbuch.Core.Interfaces
{
    /// <summary>
    /// /////////////////////////////////////////
    /// </summary>
    public interface IAuthService
    {
        Task<UserEntity?> RegisterAsync(string username, string password);
        Task<UserEntity?> ValidateCredentialsAsync(string username, string password);
    }
}