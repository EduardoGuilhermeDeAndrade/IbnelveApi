using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

/// <summary>
/// Interface base para repositórios de entidades globais
/// </summary>
//public interface IGlobalRepository<T> : IRepository<T> where T : GlobalEntity
public interface IGlobalRepository<T> : IRepository<T> where T : GlobalEntity
{
    //Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false);
    Task<IEnumerable<T>> GetActiveAsync();
}