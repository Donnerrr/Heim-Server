/*
 * Copyright (c) 2026 Nico Philipp
 * Datum: 12.06.2026 22:13:23
 * Projekt: Server.Controller
 * Datei: DebtController
 *
 * Beschreibung: Controller für die Verwaltung von Schulden. Hier werden die Endpunkte definiert, um Schulden zu erstellen, zu löschen, zu aktualisieren und abzurufen.
 */

using Microsoft.AspNetCore.Mvc;
using Schuldenbuch.Core.Interfaces;
using Schuldenbuch.Core.DTOs.DebtDtos;

namespace Server.Controller.Schuldenbuch
{

    [ApiController]
    [Route("api/Schuldenbuch/[controller]")]

    public class DebtController : ControllerBase
    {
        private readonly IDebtService _debtService;

        public DebtController(IDebtService debtService)
        {
            _debtService = debtService;
        }
        [HttpPost]
        public async Task<IActionResult> AddDebt([FromBody] AddDebtDto dto)
        {

             try
            {
                var result = await _debtService.AddDebtAsync(dto);

                
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                // Das hier schickt den ECHTEN C#-Fehlertext mitsamt Stacktrace direkt an den Browser
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"C# Crash: {innerMessage} \n\n StackTrace: {ex.StackTrace}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDebt(int id)
        {
            var result = await _debtService.DeleteDebtAsync(id);
            switch (result.Status)
            {
                case DeleteStatus.Success:
                    return Ok(result.Message);
                case DeleteStatus.NotFound:
                    return NotFound(result.Message);
                default:
                    return StatusCode(500, "Unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDebt(int id, [FromBody] string amount)
        {
            var result = await _debtService.UpdateDebtAsync(id, amount);
            switch (result.Status)
            {
                case UpdateStatus.Success:
                    return Ok(result.Message);
                case UpdateStatus.Failed:
                    return BadRequest(result.Message);
                case UpdateStatus.ValidationError:
                    return BadRequest(result.Message);
                default:
                    return StatusCode(500, "Unexpected error occurred.");
            }
        }
    }
}