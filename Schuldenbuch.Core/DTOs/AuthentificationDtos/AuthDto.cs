// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: AuthDto.cs
//  */

/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 09.07.2026 21:55:47
 * Projekt: Schuldenbuch.Core.DTOs
 * Datei: AuthDto
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

namespace Schuldenbuch.Core.DTOs.AuthentificationDtos
{
    public record LoginRegisterRequest(string Username, string Password);
    public record LoginResponse(string Token, int UserId, string Username);
}
