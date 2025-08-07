// IbnelveApi.Domain/Interfaces/IRepository.cs
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    //Task<IEnumerable<T>> GetAllIncludingDeletedAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
