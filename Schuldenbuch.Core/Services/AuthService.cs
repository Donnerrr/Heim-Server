// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: AuthService.cs
//  */

/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 09.07.2026 21:19:01
 * Projekt: Schuldenbuch.Core.Services
 * Datei: AuthService
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

using Microsoft.AspNetCore.Identity;
using Schuldenbuch.Core.Entities;
using Schuldenbuch.Core.Interfaces;


namespace Schuldenbuch.Core.Services
{
    /// <summary>
    /// /////////////////////////////////////////
    /// </summary>
    public class AuthService : IAuthService
    {

        private readonly ISchuldenbuchDatabase _db;
        private readonly PasswordHasher<UserEntity> _hasher;

        public AuthService(ISchuldenbuchDatabase db)
        {
            _db = db;
            _hasher = new PasswordHasher<UserEntity>();
        }

        //Neuen User Registrieren
        public async Task<UserEntity?> RegisterAsync(string username, string password)
        {
            var exists = await _db.GetUserByUsernameAsync(username); 
            if (exists != null)
            {
                return null; 
            }

            var user = new UserEntity { Username = username };
            user.PasswordHash = _hasher.HashPassword(user, password);

            await _db.AddUserAsync(user);

            return user;
        }

        //LogIn prüfen
        public async Task<UserEntity?> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _db.GetUserByUsernameAsync(username);
            if (user == null) { return null; }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}