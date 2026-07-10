/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 09.07.2026 21:54:25
 * Projekt: Server.Controller
 * Datei: AuthController
 *
 * Beschreibung: Füge hier eine kurze Beschreibung hinzu, was diese Klasse tut.
 */

using Microsoft.AspNetCore.Mvc;
using Schuldenbuch.Core.DTOs.AuthentificationDtos;
using Schuldenbuch.Core.Interfaces;
using Schuldenbuch.Core.Services;

namespace Server.Controller.Authentification
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthController(IAuthService authService, IJwtTokenGenerator tokenGenerator)
        {
            _authService = authService;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username und Passwort dürfen nicht leer sein.");

            var user = await _authService.RegisterAsync(request.Username, request.Password);

            if (user == null)
                return Conflict("Dieser Username ist bereits vergeben.");

            var token = _tokenGenerator.GenerateToken(user);
            return Ok(new { token, user.Id, user.Username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username und Passwort dürfen nicht leer sein.");

            var user = await _authService.ValidateCredentialsAsync(request.Username, request.Password);

            if (user == null)
                return Unauthorized("Username oder Passwort falsch.");

            var token = _tokenGenerator.GenerateToken(user);
            return Ok(new LoginResponse(token, user.Id, user.Username));
        }
    }
}