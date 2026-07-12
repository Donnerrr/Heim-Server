using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schuldenbuch.Core.DTOs.PersonDtos;
using Schuldenbuch.Core.Extensions;
using Schuldenbuch.Core.Interfaces;

namespace Server.Controller.Schuldenbuch
{
    [Authorize]
    [ApiController]
    [Route("api/Schuldenbuch/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson([FromBody] AddPersonDto dto)
        {
            // Hier holen wir die ID aus dem Token
            var userId = User.GetUserId();

            // Die Methode im Service muss diesen userId-Parameter nun auch akzeptieren
            var result = await _personService.AddPersonAsync(dto, userId);

            return result.Status switch
            {
                AddPersonStatus.Success => Ok(new { result }),
                AddPersonStatus.ValidationError => BadRequest(result),
                _ => StatusCode(500, "Unexpected error occurred.")
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var userId = User.GetUserId();
            // Auch beim Löschen sollte sichergestellt sein, dass der User die Person auch besitzen darf
            var result = await _personService.DeletePersonAsync(id);

            return result.Status switch
            {
                DeleteStatus.Success => Ok(result.Message),
                DeleteStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, "Unexpected error occurred.")
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            var userId = User.GetUserId();
            var result = await _personService.GetPersonAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            var userId = User.GetUserId();
            var persons = await _personService.GetAllPersonsAsync(userId);
            return Ok(persons);
        }
    }
}