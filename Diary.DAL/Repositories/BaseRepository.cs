using Diary.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Diary.DAL.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IQueryable<T> GetAll()
    {
        return _dbContext.Set<T>();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<T> CreateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException($"Entity - {nameof(entity)} is null");

        await _dbContext.AddAsync(entity);
        
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException($"Entity - {nameof(entity)} is null");
        
        _dbContext.Update(entity);

        return entity;
    }

    public async Task<T> DeleteAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException($"Entity - {nameof(entity)} is null");
        
        _dbContext.Remove(entity);

        return entity;
    }
}