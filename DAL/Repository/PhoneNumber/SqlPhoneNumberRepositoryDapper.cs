using Dapper;
using Microsoft.Data.SqlClient;

namespace DAL.Repository.PhoneNumber;

public class SqlPhoneNumberRepositoryDapper(SqlConnection conn) : IPhoneNumberRepository
{
    public async Task<Shared.Models.PhoneNumber> GetByIdAsync(int id)
    {
        const string query = "SELECT * FROM PhoneNumbers WHERE ID = @ID";

        return await conn.QueryFirstAsync<Shared.Models.PhoneNumber>(query, new { ID = id });
    }

    public async Task AddAsync(Shared.Models.PhoneNumber phoneNumber)
    {
        const string query = "INSERT INTO PhoneNumbers (PersonID, Number, NumberType) VALUES (@PersonID, @Number, @NumberType)";

        await conn.ExecuteAsync(query, phoneNumber);
    }

    public async Task UpdateAsync(Shared.Models.PhoneNumber updatedPhoneNumber)
    {
        const string query =
            "UPDATE PhoneNumbers SET PersonID = @PersonID, Number = @Number, NumberType = @NumberType WHERE ID = @ID";

        await conn.ExecuteAsync(query, updatedPhoneNumber);
    }

    public async Task DeleteAsync(int id)
    {
        const string query = "DELETE FROM PhoneNumbers Where ID = @ID";

        await conn.ExecuteAsync(query, new { ID = id });
    }

    public async Task<IEnumerable<Shared.Models.PhoneNumber>> GetAllAsync()
    {
        const string query = "SELECT * FROM PhoneNumbers";

        return await conn.QueryAsync<Shared.Models.PhoneNumber>(query);
    }

    public async Task DeletePhoneNumberByIdsAsync(int personId, int phoneNumberId)
    {
        const string query = "DELETE FROm PhoneNumbers WHERE PersonID = @PersonID, ID = @ID";

        await conn.ExecuteAsync(query, new { PersonID = personId, ID = phoneNumberId });

    }

    public async Task<Shared.Models.PhoneNumber> GetByPhoneNumberAsync(string phoneNumber)
    {
        const string query = "SELECT * FROM PhoneNumbers WHERE Number = @Number";

        return await conn.QueryFirstAsync<Shared.Models.PhoneNumber>(query, new { Number = phoneNumber });
    }
}