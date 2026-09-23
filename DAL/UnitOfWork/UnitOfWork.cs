using DAL.Database;
using DAL.Repository.Person;
using DAL.Repository.PhoneNumber;
using DAL.Repository.RelatedPerson;

namespace DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IPersonRepository Persons { get; }
    IRelatedPersonRepository RelatedPersons { get; }
    IPhoneNumberRepository PhoneNumbers { get; }
    Task SaveAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context, IPersonRepository personRepository, IRelatedPersonRepository relatedPersons, IPhoneNumberRepository phoneNumbers)
    {
        _context = context;
        Persons = personRepository;
        RelatedPersons = relatedPersons;
        PhoneNumbers = phoneNumbers;
    }
    public IPersonRepository Persons { get; }
    public IRelatedPersonRepository RelatedPersons { get; }
    public IPhoneNumberRepository PhoneNumbers { get; }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        _context.DisposeAsync();
    }
}
