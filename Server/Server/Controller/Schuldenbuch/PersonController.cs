using Microsoft.AspNetCore.Mvc;
using Schuldenbuch.Core.DTOs.PersonDtos;
using Schuldenbuch.Core.Interfaces;



namespace Server.Controller.Schuldenbuch
{

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
        public async Task<IActionResult> AddPerson([FromBody]AddPersonDto dto)
        {
            var result =  await _personService.AddPersonAsync(dto);

            return result.Status switch
            {
                AddPersonStatus.Success => Ok(new {Message = result.Message }),
                AddPersonStatus.ValidationError => BadRequest(result.Message),
                _ => StatusCode(500, "Unexpected error occurred.")
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var result =  await _personService.DeletePersonAsync(id);

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
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
            var persons =  await _personService.GetAllPersonsAsync();

            return Ok(persons);

        }



    }



}
