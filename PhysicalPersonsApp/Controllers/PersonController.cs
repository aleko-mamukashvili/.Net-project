using BAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhysicalPersonsApp.Helpers;
using Shared.DTO;

namespace PhysicalPersonsApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PersonController : Controller
{

    private readonly ILogger<PersonController> _logger;
    private readonly IFileService _fileService;
    private readonly IPersonService _personService;
    private readonly string[] _allowedFileExtensions = [".jpg", ".jpeg", ".png"];

    public PersonController(ILogger<PersonController> logger, IFileService fileService, IPersonService personService)
    {
        _logger = logger;
        _fileService = fileService;
        _personService = personService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPersons()
    {
        var persons = await _personService.GetAllPersons();

        return Ok(persons);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetPersonById(int id)
    {
        var person = await _personService.GetPersonAsync(id);

        if (person == null!) return NotFound("Person not found");

        return Ok(person);
    }


    [HttpGet]
    [Route("Report/{id:int}")]
    public async Task<IActionResult> GetReportByPersonId(int id)
    {
        var report = await _personService.GetRelatedPersonReport(id);

        return Ok(report);
    }

    [HttpGet]
    [Route("Search")]
    public async Task<IActionResult> SearchPersonByWord([FromQuery] string searchWord, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
    {
        var response = await _personService.GetByFields(searchWord);

        return Ok(response);
    }


    [HttpPost]
    public async Task<IActionResult> AddPerson([FromForm] AddPersonDto personObj, [FromServices] IFileFactory fileFactory)
    {
        if (personObj == null!)
        {
            return BadRequest("Person data is null.");
        }

        var imagePath = await _fileService.SaveFileAsync(fileFactory.CreateFile(personObj.Image), _allowedFileExtensions);

        try
        {
            await _personService.CreatePersonAsync(personObj, imagePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding a person.");
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
        return Ok(new { Message = "Person created successfully" });
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdatePerson(int id, [FromForm] UpdatePersonDto? personObj, [FromServices] IFileFactory fileFactory)
    {
        var person = await _personService.GetPersonAsync(id);

        if (person == null!)
        {
            return NotFound("Person not found");
        }

        if (personObj == null!)
        {
            return BadRequest("Person data is null.");
        }

        var imagePath = personObj.Image != null! ? await _fileService.UpdateFileAsync(person.Image, fileFactory.CreateFile(personObj.Image), new string[] { ".jpg", ".jpeg", ".png" }) : person.Image;
        
        try
        {
            await _personService.UpdatePersonAsync(personObj, person, imagePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating a person.");
            return StatusCode(500, "Internal server error: " + ex.Message);
        }

        return Ok(new { Message = "Person updated successfully", person });
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var person = await _personService.GetPersonAsync(id);

        try
        {
            if (person == null!)
            {
                return NotFound("Person not found");
            }

            await _personService.DeletePersonAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting a person.");
            return StatusCode(500, "Internal server error: " + ex.Message);
        }

        return Ok(new { Message = "Person deleted successfully", person });
    }
}
