using DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.RelatedPerson;

public interface IRelatedPersonRepository : IRepository<Shared.Models.RelatedPerson>
{
    
    public Task<Shared.Models.RelatedPerson?> GetByIdsAsync(int personId, int relatedPersonId);
    public Task DeleteByIdsAsync(int personId, int relatedPersonId);

}


public class SqlRelatedPersonRepository(AppDbContext context) : IRelatedPersonRepository
{
    public async Task<Shared.Models.RelatedPerson> GetByIdAsync(int id)
    {
        var result = await context.RelatedPersons.Include(rp => rp.PersonRelated)
            .Include(rp => rp.RelatedPersonPerson)
            .FirstOrDefaultAsync(rp => rp.RelatedPersonId == id);

        return result!;
    }

    public async Task AddAsync(Shared.Models.RelatedPerson relatedPerson)
    {
        await context.RelatedPersons.AddAsync(relatedPerson);
    }

    public Task UpdateAsync(Shared.Models.RelatedPerson relatedPerson)
    {
        context.RelatedPersons.Update(relatedPerson);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var relatedPersonToRemove = await GetByIdAsync(id);

        context.RelatedPersons.Remove(relatedPersonToRemove);
    }

    public async Task<IEnumerable<Shared.Models.RelatedPerson>> GetAllAsync()
    {
        return await context.RelatedPersons.Include(rp => rp.PersonRelated)
                                            .Include(rp => rp.RelatedPersonPerson)
                                            .ToListAsync();
    }

    public async Task<Shared.Models.RelatedPerson?> GetByIdsAsync(int personId, int relatedPersonId)
    {
        var relatedPerson = await context.RelatedPersons.FirstOrDefaultAsync(rp => rp.RelatedPersonPersonId == personId && rp.PersonRelatedId == relatedPersonId);
        return relatedPerson;
    }

    public async Task DeleteByIdsAsync(int personId, int relatedPersonId)
    {
        var relatedPersonToRemove = await context.RelatedPersons.FirstOrDefaultAsync(rp => rp.RelatedPersonPersonId == personId && rp.PersonRelatedId == relatedPersonId);
        if (relatedPersonToRemove == null)
        {
            throw new Exception("Related person not found");
        }

        context.RelatedPersons.Remove(relatedPersonToRemove);
    }
}
