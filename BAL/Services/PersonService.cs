using DAL.UnitOfWork;
using Shared.DTO;
using Shared.Models;
using Shared.Paging;

namespace BAL.Services;

public interface IPersonService
{
    Task<Person> GetPersonAsync(int id);
    Task<IEnumerable<Person>> GetAllPersons();
    Task<PaginatedResponse<Person>> GetByFields(string searchInput, int pageNumber = 1, int pageSize = 5);
    Task CreatePersonAsync(AddPersonDto personObj, string filePath);
    Task UpdatePersonAsync(UpdatePersonDto? personObj, Person person, string filePath);
    Task DeletePersonAsync(int id);
    Task<Report> GetRelatedPersonReport(int id);
}


public class PersonService(IUnitOfWork unitOfWork) : IPersonService
{
    public async Task<Person> GetPersonAsync(int id)
    {
        return await unitOfWork.Persons.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Person>> GetAllPersons()
    {
        return await unitOfWork.Persons.GetAllAsync();
    }

    public async Task<PaginatedResponse<Person>> GetByFields(string searchInput, int pageNumber = 1, int pageSize = 5)
    {
        var persons = await unitOfWork.Persons.GetByFields(searchInput);

        var enumerable = persons.ToList();

        var totalRecords = enumerable.Count();

        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        if (pageNumber < 1)
            pageNumber = 1;

        if (pageNumber > totalPages)
            pageNumber = totalPages;

        var personsResult = enumerable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        var response = new PaginatedResponse<Person>
        {
            CurrentPage = pageNumber,
            TotalPages = totalPages,
            TotalRecords = totalRecords,
            Items = personsResult
        };


        return response;
    }

    public async Task CreatePersonAsync(AddPersonDto personObj, string filePath)
    {
        var person = new Person
        {
            Name = personObj.Name,
            Surname = personObj.Surname,
            BirthDate = personObj.BirthDate,
            City = new City { CityName = personObj.City.Name! },
            Gender = personObj.Gender,
            Image = filePath,
            PersonalNumber = personObj.PersonalNumber,
            PhoneNumber = [],
            RelatedPerson = []
        };

        foreach (var rp in person.RelatedPerson)
        {
            await unitOfWork.RelatedPersons.AddAsync(rp);
        }

        await unitOfWork.Persons.AddAsync(person);
        await unitOfWork.SaveAsync();
    }

    public async Task UpdatePersonAsync(UpdatePersonDto? personObj,Person person, string filePath)
    {

        person.Name = personObj!.Name ?? person.Name;
        person.Surname = personObj.Surname ?? person.Surname;
        person.BirthDate = personObj.BirthDate ?? person.BirthDate;
        person.City = new City { CityName = personObj.City!.Name! };
        person.Gender = personObj.Gender ?? person.Gender;
        person.PersonalNumber = personObj.PersonalNumber ?? person.PersonalNumber;
        person.PhoneNumber = [];
        person.RelatedPerson = [];



        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveAsync();
    }

    public async Task DeletePersonAsync(int id)
    {
        await unitOfWork.Persons.DeleteAsync(id);
        await unitOfWork.SaveAsync();
    }

    public async Task<Report> GetRelatedPersonReport(int id)
    {
        var personReportDict = new Dictionary<string, int>();

        var person = await unitOfWork.Persons.GetByIdAsync(id);

        personReportDict.Add("კოლეგა", person.RelatedPerson.Count(rp => rp.PersonType.Equals("კოლეგა")));
        personReportDict.Add("ნაცნობი", person.RelatedPerson.Count(rp => rp.PersonType.Equals("ნაცნობი")));
        personReportDict.Add("ნათესავი", person.RelatedPerson.Count(rp => rp.PersonType.Equals("ნათესავი")));
        personReportDict.Add("სხვა", person.RelatedPerson.Count(rp => rp.PersonType.Equals("სხვა")));


        return new Report { PersonReport = personReportDict };
    }
}