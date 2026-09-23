using DAL.UnitOfWork;
using Shared.Models;

namespace BAL.Services;

public interface IRelatedPersonService
{
    Task CreateRelatedPersonAsync(RelatedPerson relatedPerson);
    Task DeleteByIdsAsync(int personId, int relatedPersonId);
    Task<RelatedPerson?> GetRelatedPersonByIdsAsync(int personId, int relatedPersonId);
    Task<IEnumerable<RelatedPerson>> GetAll();
}


public class RelatedPersonService(IUnitOfWork unitOfWork) : IRelatedPersonService
{
    public async Task<RelatedPerson?> GetRelatedPersonByIdsAsync(int personId, int relatedPersonId)
    {
        return await unitOfWork.RelatedPersons.GetByIdsAsync(personId, relatedPersonId);
    }

    public async Task<IEnumerable<RelatedPerson>> GetAll()
    {
        return await unitOfWork.RelatedPersons.GetAllAsync();
    }

    public async Task CreateRelatedPersonAsync(RelatedPerson relatedPerson)
    {

        await unitOfWork.RelatedPersons.AddAsync(relatedPerson);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteByIdsAsync(int personId, int relatedPersonId)
    {
        await unitOfWork.RelatedPersons.DeleteByIdsAsync(personId, relatedPersonId);
        await unitOfWork.SaveAsync();
    }
}
