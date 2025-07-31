using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id, string tenantId);
    Task<IEnumerable<T>> GetAllAsync(string tenantId, bool includeDeleted = false);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id, string tenantId);
    Task<bool> ExistsAsync(int id, string tenantId);
}

