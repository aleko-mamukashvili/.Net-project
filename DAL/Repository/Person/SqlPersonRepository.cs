using DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Person;

public interface IPersonRepository : IRepository<Shared.Models.Person>
{
    Task<IEnumerable<Shared.Models.Person>> GetByFields(string searchInput);
}

public class SqlPersonRepository(AppDbContext context) : IPersonRepository
{
    public async Task<Shared.Models.Person> GetByIdAsync(int id)
    {
        var result = await context.Persons.Include(c => c.City)
                                 .Include(pn => pn.PhoneNumber)
                                 .Include(rp => rp.RelatedPerson).ThenInclude(rp => rp.PersonRelated)
                                 .FirstOrDefaultAsync(p => p.PersonId == id);
        return result!;
    }

    public async Task AddAsync(Shared.Models.Person person)
    {
        var checkedPerson = await CheckForUniqueValues(person);

        await context.Persons.AddAsync(checkedPerson);
    }

    public async Task UpdateAsync(Shared.Models.Person person)
    {
        var checkedPerson = await CheckForUniqueValues(person);

        context.Persons.Update(checkedPerson);
    }

    public async Task DeleteAsync(int id)
    {
        var personToRemove = await GetByIdAsync(id);

        if (personToRemove != null!)
        {
            context.Persons.Remove(personToRemove);
        }
    }

    public async Task<IEnumerable<Shared.Models.Person>> GetAllAsync()
    {
        var result = await context.Persons.Include(c => c.City)
                                                      .Include(pn => pn.PhoneNumber)
                                                      .Include(rp => rp.RelatedPerson)
                                                      .ThenInclude(rp => rp.PersonRelated).ToListAsync();
        return result;
    }

    private async Task<Shared.Models.Person> CheckForUniqueValues(Shared.Models.Person person)
    {
        var checkCity = await context.Cities.FirstOrDefaultAsync(c => c.CityName == person.City.CityName);
        var checkPhoneNumberList = new List<Shared.Models.PhoneNumber>();
        var checkRelatedPersonList = new List<Shared.Models.RelatedPerson>();

        foreach (var phoneNumber in person.PhoneNumber)
        {
            var checkPhoneNumber = await context.PhoneNumbers.FirstOrDefaultAsync(pn => pn.Number == phoneNumber.Number);

            if (checkPhoneNumber is not null)
            {
                checkPhoneNumberList.Add(checkPhoneNumber);
            }
            else
            {
                checkPhoneNumberList.Add(phoneNumber);
            }
        }

        if (checkCity is not null)
        {
            person.City = checkCity;
        }

        person.PhoneNumber = checkPhoneNumberList;
        person.RelatedPerson = checkRelatedPersonList;

        return person;
    }

    public async Task<IEnumerable<Shared.Models.Person>> GetByFields(string searchInput)
    {
        var result = await context.Persons.Include(c => c.City)
                                                 .Include(pn => pn.PhoneNumber)
                                                 .Include(rp => rp.RelatedPerson)
                                                 .Where(p => p.Name.Contains(searchInput) ||
                                                                   p.Surname.Contains(searchInput) ||
                                                                   p.PersonalNumber.Contains(searchInput)).ToListAsync();

        return result;
    }
}
