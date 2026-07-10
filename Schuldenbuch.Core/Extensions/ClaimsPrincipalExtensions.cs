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
            // Wir suchen explizit nach dem String, der auch in deinem Token steht
            var idClaim = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            // Falls das nicht greift, versuchen wir es mit dem Kurznamen (manchmal registriert ASP.NET das um)
            if (idClaim == null)
            {
                idClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            if (idClaim == null || !int.TryParse(idClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User-ID konnte nicht aus dem Token extrahiert werden.");
            }

            return userId;
        }
    }
}
