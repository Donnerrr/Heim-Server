// /*
//  * Copyright (c) 2025 Nico Philipp * Datei: JwtTokenGenerator.cs
//  */

/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 09.07.2026 21:52:25
 * Projekt: Schuldenbuch.Core.Services
 * Datei: JwtTokenGenerator
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Schuldenbuch.Core.Entities;
using Schuldenbuch.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Schuldenbuch.Core.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserEntity user);
    }

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string _secret;

        public JwtTokenGenerator(IConfiguration config)
        {
            _secret = config["Jwt:Secret"]
                ?? throw new InvalidOperationException("Jwt:Secret fehlt in appsettings.json");
        }

        public string GenerateToken(UserEntity user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}