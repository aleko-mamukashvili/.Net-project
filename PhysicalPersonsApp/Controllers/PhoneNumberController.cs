using BAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using Shared.Models;

namespace PhysicalPersonsApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class PhoneNumberController : ControllerBase
{
    private readonly ILogger<RelatedPersonController> _logger;
    private readonly IPhoneNumberService _phoneNumberService;
    private readonly IPersonService _personService;

    public PhoneNumberController(ILogger<RelatedPersonController> logger, IPhoneNumberService phoneNumberService, IPersonService personService)
    {
        _logger = logger;
        _phoneNumberService = phoneNumberService;
        _personService = personService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _phoneNumberService.GetAllPhoneNumbersAsync());
    }

    [HttpPost]
    public async Task<IActionResult> AddRelation(PhoneNumberDto phoneNumberObj)
    {
        var testRelation = await _phoneNumberService.GetPhoneNumberByPhoneNumberAsync(phoneNumberObj.Number);

        if (testRelation == null)

        {
            var person = await _personService.GetPersonAsync(phoneNumberObj.PersonId);


            await _phoneNumberService.CreatePhoneNumberAsync(new PhoneNumber
            {
                Person = person,
                Number = phoneNumberObj.Number,
                NumberType = phoneNumberObj.NumberType,
                PhonePersonId = phoneNumberObj.PersonId
            });

            return Ok(new { Message = "Phone number successfully added!" });
        }

        _logger.LogError("Phone number already exists!");
        return BadRequest(new { Message = "Phone number already exists!" });
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePhoneNumber(DeletePhoneNumberDto phoneNumberObj)
    {
        await _phoneNumberService.DeleteByPersonIdsAsync(phoneNumberObj.PersonId, phoneNumberObj.PhoneNumberId);

        return Ok(new { Message = "Phone number successfully DeletedDeleted!" });
    }
}