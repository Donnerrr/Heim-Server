// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: ClaimsPrincipalExtensions.cs
//  */

/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 09.07.2026 22:42:35
 * Projekt: Schuldenbuch.Core.Extensions
 * Datei: ClaimsPrincipalExtensions
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

using System.Security.Claims;

namespace Schuldenbuch.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (idClaim == null || !int.TryParse(idClaim, out var userId))
                throw new UnauthorizedAccessException("Kein gültiger User-Claim im Token.");
            return userId;
        }
    }
}
