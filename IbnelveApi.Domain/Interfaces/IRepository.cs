using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

/// <summary>
/// Interface base para repositórios - requer que T herde de GlobalEntity
/// </summary>
public interface IRepository<T> where T : GlobalEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

