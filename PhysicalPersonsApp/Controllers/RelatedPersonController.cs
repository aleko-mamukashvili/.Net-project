using BAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using Shared.Models;

namespace PhysicalPersonsApp.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RelatedPersonController : Controller
{

    private readonly ILogger<RelatedPersonController> _logger;
    private readonly IRelatedPersonService _relatedPersonService;
    private readonly IPersonService _personService;

    public RelatedPersonController(ILogger<RelatedPersonController> logger, IRelatedPersonService relatedPersonService, IPersonService personService)
    {
        _logger = logger;
        _relatedPersonService = relatedPersonService;
        _personService = personService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _relatedPersonService.GetAll();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddRelation(RelatedPersonDto relatedPersonObj)
    {
        var testRelation =
            await _relatedPersonService.GetRelatedPersonByIdsAsync(relatedPersonObj.PersonId,
                relatedPersonObj.PersonRelatedId);

        if (testRelation == null)

        {
            var person = await _personService.GetPersonAsync(relatedPersonObj.PersonId);
            var relatedPerson = await _personService.GetPersonAsync(relatedPersonObj.PersonRelatedId);


            await _relatedPersonService.CreateRelatedPersonAsync(new RelatedPerson
            {
                RelatedPersonPerson = person,
                PersonRelated = relatedPerson,
                PersonType = relatedPersonObj.PersonType,
                RelatedPersonPersonId = relatedPersonObj.PersonId,
                PersonRelatedId = relatedPersonObj.PersonRelatedId
            });

            return Ok(new { Message = "Relation successfully added!" });
        }

        _logger.LogError("Relation already exists!");
        return BadRequest(new { Message = "Relation already exists!" });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteRelation(DeletePersonRelationDto relatedPersonObj)
    {
        await _relatedPersonService.DeleteByIdsAsync(relatedPersonObj.PersonId, relatedPersonObj.RelatedPersonId);

        return Ok(new { Message = "Relation successfully deleted!" });
    }
}