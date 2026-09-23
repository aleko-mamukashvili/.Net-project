using DAL.UnitOfWork;
using Shared.Models;

namespace BAL.Services;

public interface IPhoneNumberService
{
    Task<PhoneNumber?> GetPhoneNumberByPhoneNumberAsync(string phoneNumber);
    Task CreatePhoneNumberAsync(PhoneNumber relatedPerson);
    Task DeleteByPersonIdsAsync(int personId, int phoneNumberId);
    Task<IEnumerable<PhoneNumber>> GetAllPhoneNumbersAsync();
}


public class PhoneNumberService(IUnitOfWork unitOfWork) : IPhoneNumberService
{
    public async Task<PhoneNumber?> GetPhoneNumberByPhoneNumberAsync(string phoneNumber)
    {
        return await unitOfWork.PhoneNumbers.GetByPhoneNumberAsync(phoneNumber);
    }

    public async Task CreatePhoneNumberAsync(PhoneNumber phoneNumber)
    {
        await unitOfWork.PhoneNumbers.AddAsync(phoneNumber);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteByPersonIdsAsync(int personId, int phoneNumberId)
    {
        await unitOfWork.PhoneNumbers.DeletePhoneNumberByIdsAsync(personId, phoneNumberId);
        await unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<PhoneNumber>> GetAllPhoneNumbersAsync()
    {
        return await unitOfWork.PhoneNumbers.GetAllAsync();
    }
}