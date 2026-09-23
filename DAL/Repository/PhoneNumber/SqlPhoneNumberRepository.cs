using DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.PhoneNumber;

public interface IPhoneNumberRepository : IRepository<Shared.Models.PhoneNumber>
{
    public Task DeletePhoneNumberByIdsAsync(int personId, int phoneNumberId);
    public Task<Shared.Models.PhoneNumber> GetByPhoneNumberAsync(string phoneNumber);

}


public class SqlPhoneNumberRepository(AppDbContext context) : IPhoneNumberRepository
{
    public async Task<Shared.Models.PhoneNumber> GetByIdAsync(int id)
    {
        var result = await context.PhoneNumbers.FirstOrDefaultAsync(pn => pn.PhoneId == id);

        return result!;
    }

    public async Task AddAsync(Shared.Models.PhoneNumber relatedPerson)
    {
        await context.PhoneNumbers.AddAsync(relatedPerson);
    }

    public Task UpdateAsync(Shared.Models.PhoneNumber relatedPerson)
    {
        context.PhoneNumbers.Update(relatedPerson);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var phoneNumberToRemove = await GetByIdAsync(id);

        context.PhoneNumbers.Remove(phoneNumberToRemove);
    }

    public async Task<IEnumerable<Shared.Models.PhoneNumber>> GetAllAsync()
    {
        return await context.PhoneNumbers.ToListAsync();
    }

    public async Task DeletePhoneNumberByIdsAsync(int personId, int phoneNumberId)
    {
        var phoneNumberToRemove = await context.PhoneNumbers.FirstOrDefaultAsync(rp => rp.PhonePersonId == personId && rp.PhoneId == phoneNumberId);
        if (phoneNumberToRemove == null)
        {
            throw new Exception("Phone number not found");
        }

        context.PhoneNumbers.Remove(phoneNumberToRemove);
    }

    public async Task<Shared.Models.PhoneNumber> GetByPhoneNumberAsync(string phoneNumber)
    {
        var result = await context.PhoneNumbers.FirstOrDefaultAsync(pn => pn.Number == phoneNumber);
        return result!;
    }
}
