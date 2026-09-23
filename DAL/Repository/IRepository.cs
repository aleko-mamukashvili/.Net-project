namespace DAL.Repository;

public interface IRepository<T>
{
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T obj);
    Task UpdateAsync(T updatedObj);
    Task DeleteAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}