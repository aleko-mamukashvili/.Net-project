using Dapper;
using Microsoft.Data.SqlClient;

namespace DAL.Repository.RelatedPerson;

public class SqlRelatedPersonRepositoryDapper(SqlConnection conn) : IRelatedPersonRepository
{
    public async Task<Shared.Models.RelatedPerson> GetByIdAsync(int id)
    {
        const string query = "SELECT [ID], [PersonID], [RelatedPersonID] AS PersonRelatedID, [PersonType] FROM RelatedPersons";

        return await conn.QueryFirstAsync<Shared.Models.RelatedPerson>(query);

    }

    public async Task AddAsync(Shared.Models.RelatedPerson relatedPerson)
    {
        const string query = "INSERT INTO RelatedPersons (PersonID, RelatedPersonID, PersonType) VALUES (@PersonID, @RelatedPersonID, @PersonType)";

        await conn.ExecuteAsync(query, new {PersonID = relatedPerson.RelatedPersonPersonId,
                                                 RelatedPersonID = relatedPerson.PersonRelatedId,
                                                 PersonType = relatedPerson.PersonType});
    }

    public async Task UpdateAsync(Shared.Models.RelatedPerson updatedRelatedPerson)
    {
        const string query = "UPDATE RelatedPersons SET PersonID = @PersonID, RelatedPersonID = @RelatedPersonID, PersonType = @PersonType WHERE ID = @ID";
        await conn.ExecuteAsync(query,
            new
            {
                PersonID = updatedRelatedPerson.RelatedPersonPersonId,
                RelatedPersonID = updatedRelatedPerson.PersonRelatedId,
                PersonType = updatedRelatedPerson.PersonType,
                ID = updatedRelatedPerson.RelatedPersonId
            });
    }

    public async Task DeleteAsync(int id)
    {
        const string query = "DELETE FROM RelatedPersons WHERE ID = @ID";

        await conn.ExecuteAsync(query, new { ID = id });
    }

    public async Task<IEnumerable<Shared.Models.RelatedPerson>> GetAllAsync()
    {
        const string query = "SELECT [ID], [PersonID], [RelatedPersonID] AS PersonRelatedID, [PersonType] FROM RelatedPersons";

        return await conn.QueryAsync<Shared.Models.RelatedPerson>(query);
    }

    public async Task<Shared.Models.RelatedPerson?> GetByIdsAsync(int personId, int relatedPersonId)
    {
        const string query = """
                             SELECT [ID], [PersonID], [RelatedPersonID] AS PersonRelatedID, [PersonType] FROM RelatedPersons
                             WHERE PersonID = @PersonID AND RelatedPersonID = @RelatedPersonID
                             """;

        var result = await conn.QueryFirstOrDefaultAsync<Shared.Models.RelatedPerson>(query,
            new { PersonID = personId, RelatedPersonID = relatedPersonId });
        return result;
    }

    public async Task DeleteByIdsAsync(int personId, int relatedPersonId)
    {
        const string query = "DELETE FROM RelatedPersons WHERE PersonID = @PersonID AND RelatedPersonID = @RelatedPersonID";

        await conn.ExecuteAsync(query, new { PersonID = personId, RelatedPersonID = relatedPersonId });
    }
}