using Dapper;

namespace DAL.Repository.Person;
using Microsoft.Data.SqlClient;
using Shared.Models;

public class SqlPersonRepositoryDapper(SqlConnection conn) : IPersonRepository
{
    public async Task<Person> GetByIdAsync(int id)
    {
        const string query = @"
                                SELECT 
                                        p.Id AS PersonId, p.Name, p.Surname, p.Gender, p.PersonalNumber, p.BirthDate, p.Image,
                                        c.Id AS CityId, c.Name AS CityName,
                                        ph.Id AS PhoneId, ph.Number, ph.NumberType, ph.PersonId AS PhonePersonId,
                                        rp.Id AS RelatedPersonId, rp.PersonId AS RelatedPersonPersonId, rp.RelatedPersonID as PersonRelatedId, rp.PersonType
                                    FROM Persons p
                                    LEFT JOIN Cities c ON p.CityId = c.Id
                                    LEFT JOIN PhoneNumbers ph ON p.Id = ph.PersonId
                                    LEFT JOIN RelatedPersons rp ON p.Id = rp.PersonId
                                    WHERE p.Id = @ID;";


        var person =
            await conn.QueryAsync<Person, City, PhoneNumber, RelatedPerson,
                Person>(query, (person, city, phoneNumber, relatedPerson) =>
                {
                    var currentPerson = person;
                    currentPerson.City = city;
                    currentPerson.PhoneNumber = new List<PhoneNumber>();
                    currentPerson.RelatedPerson = new List<RelatedPerson>();

                    if (phoneNumber != null!)
                    {
                        currentPerson.PhoneNumber.Add(phoneNumber);
                    }

                    if (relatedPerson != null!)
                    {
                        if (currentPerson.RelatedPerson.All(rp => rp.PersonRelatedId != relatedPerson.PersonRelatedId))
                        {
                            currentPerson.RelatedPerson.Add(relatedPerson);
                        }
                    }

                    return currentPerson;
                },
                new { ID = id },
                splitOn: "CityId,PhoneId,RelatedPersonId");

        return person.First();
    }

    public async Task AddAsync(Person person)
    {
        const string addCityQuery = @"INSERT INTO Cities(Name) VALUES (@Name)";

        await conn.ExecuteAsync(addCityQuery, new { Name = person.City.CityName });

        const string getCityQuery = $"SELECT TOP 1 ID AS CityId, Name AS CityName FROM Cities ORDER BY Id DESC";

        var city = await conn.QueryFirstAsync<City>(getCityQuery);

        const string query = @"INSERT INTO Persons(Name, Surname, Gender, PersonalNumber, BirthDate, Image, CityID) 
                               VALUES (@Name, @Surname, @Gender, @PersonalNumber, @BirthDate, @Image, @CityID)";


        await conn.ExecuteAsync(query, new {
            person.Name,
            person.Surname,
            person.Gender,
            person.PersonalNumber,
            person.BirthDate,
            person.Image, 
            CityID = city.CityId });
    }

    public async Task UpdateAsync(Person updatedPerson)
    {
        const string query =
            @"UPDATE Persons SET Name = @Name, Surname = @Surname, Gender = @Gender, PersonalNumber = @PersonalNumber,
              BirthDate = @BirthDate, Image = @Image, CityID = @CityID";

        await conn.ExecuteAsync(query, new {
            updatedPerson.Name,
            updatedPerson.Surname,
            updatedPerson.Gender,
            updatedPerson.PersonalNumber,
            updatedPerson.BirthDate,
            updatedPerson.Image, 
            CityID = updatedPerson.City.CityId });
    }

    public async Task DeleteAsync(int id)
    {
        const string query = "DELETE FROM Persons WHERE ID = @ID";

        await conn.ExecuteAsync(query, new { ID = id });
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        const string query = @"
                                SELECT 
                                    p.Id AS PersonId, p.Name, p.Surname, p.Gender, p.PersonalNumber, p.BirthDate, p.Image,
                                    c.Id AS CityId, c.Name AS CityName,
                                    ph.Id AS PhoneId, ph.Number, ph.NumberType, ph.PersonId AS PhonePersonId,
                                    rp.Id AS RelatedPersonId, rp.PersonId AS RelatedPersonPersonId, rp.RelatedPersonID as PersonRelatedId, rp.PersonType
                                FROM Persons p
                                LEFT JOIN Cities c ON p.CityId = c.Id
                                LEFT JOIN PhoneNumbers ph ON p.Id = ph.PersonId
                                LEFT JOIN RelatedPersons rp ON p.Id = rp.PersonId;";


        var persons =
            await conn.QueryAsync<Person, City, PhoneNumber, RelatedPerson,
                Person>(query, (person, city, phoneNumber, relatedPerson) =>
            {
            
                var currentPerson = person;
                currentPerson.City = city;
                currentPerson.PhoneNumber = new List<PhoneNumber>();
                currentPerson.RelatedPerson = new List<RelatedPerson>();

                if (phoneNumber != null!)
                {
                    currentPerson.PhoneNumber.Add(phoneNumber);
                }

                if (relatedPerson != null!)
                {
                    if (currentPerson.RelatedPerson.All(rp => rp.PersonRelatedId != relatedPerson.PersonRelatedId))
                    {
                        currentPerson.RelatedPerson.Add(relatedPerson);
                    }
                }
                return currentPerson;
            },
            splitOn: "CityId,PhoneId,RelatedPersonId");

        return persons;
    }

    public async Task<IEnumerable<Person>> GetByFields(string searchInput)
    {
        const string query = @"
        SELECT 
            p.Id AS PersonId, p.Name, p.Surname, p.Gender, p.PersonalNumber, p.BirthDate, p.Image,
            c.Id AS CityId, c.Name AS CityName,
            ph.Id AS PhoneId, ph.Number, ph.NumberType, ph.PersonId AS PhonePersonId,
            rp.Id AS RelatedPersonId, rp.PersonId AS RelatedPersonPersonId, rp.RelatedPersonID as PersonRelatedId, rp.PersonType
        FROM Persons p
        LEFT JOIN Cities c ON p.CityId = c.Id
        LEFT JOIN PhoneNumbers ph ON p.Id = ph.PersonId
        LEFT JOIN RelatedPersons rp ON p.Id = rp.PersonId
        WHERE p.Name LIKE @SearchPattern OR p.Surname LIKE @SearchPattern OR p.PersonalNumber LIKE @SearchPattern;";

        var personDictionary = new Dictionary<int, Person>();

        var persons = await conn.QueryAsync<Person, City, PhoneNumber, RelatedPerson, Person>(
            query,
            (person, city, phoneNumber, relatedPerson) =>
            {
                if (!personDictionary.TryGetValue(person.PersonId, out var currentPerson))
                {
                    currentPerson = person;
                    currentPerson.City = city;
                    currentPerson.PhoneNumber = new List<PhoneNumber>();
                    currentPerson.RelatedPerson = new List<RelatedPerson>();
                    personDictionary.Add(currentPerson.PersonId, currentPerson);
                }

                if (phoneNumber != null! && currentPerson.PhoneNumber.All(p => p.PhoneId != phoneNumber.PhoneId))
                {
                    currentPerson.PhoneNumber.Add(phoneNumber);
                }

                if (relatedPerson != null! && currentPerson.RelatedPerson.All(r => r.RelatedPersonId != relatedPerson.RelatedPersonId))
                {
                    currentPerson.RelatedPerson.Add(relatedPerson);
                }

                return currentPerson;
            },
            new { SearchPattern = $"%{searchInput}%" },
            splitOn: "CityId,PhoneId,RelatedPersonId");

        return personDictionary.Values;
    }

}