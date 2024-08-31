using Diary.Domain.Interfaces.Databases;

namespace Diary.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> : IStateSaveChanges
{
    IQueryable<T> GetAll();
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}