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
using Microsoft.AspNetCore.Authorization;

namespace Server.Controller.Schuldenbuch
{
    [Authorize]
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

                
                return Ok(result);
            }
            catch (Exception ex)
{
    // Fehler loggen für dich
    Console.Error.WriteLine($"Fehler in AddDebt: {ex.Message}\n{ex.StackTrace}");
    
    // Aber dem Client nur generische Nachricht
    return StatusCode(500, "Ein interner Fehler ist aufgetreten. Bitte versuchen Sie es später erneut.");
}
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDebt(int id)
        {
            var result = await _debtService.DeleteDebtAsync(id);
            switch (result.Status)
            {
                case DeleteStatus.Success:
                    return Ok(result);
                case DeleteStatus.NotFound:
                    return NotFound(result);
                default:
                    return StatusCode(500, "Unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDebt(int id, [FromBody] UpdateDebtRequestDto request)
        {
            var result = await _debtService.UpdateDebtAsync(id, request.Amount);
            switch (result.Status)
            {
                case UpdateStatus.Success:
                    return Ok(result);
                case UpdateStatus.Failed:
                    return BadRequest(result);
                case UpdateStatus.ValidationError:
                    return BadRequest(result);
                default:
                    return StatusCode(500, "Unexpected error occurred.");
            }
        }
    }
}