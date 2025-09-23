using IbnelveApi.Domain.Entities.General;

namespace IbnelveApi.Domain.Interfaces;



/// <summary>
/// Interface base para repositórios de entidades do tenant
/// </summary>
public interface ITenantRepository<T> : IRepository<T> where T : TenantEntity
{
    Task<T?> GetByIdAsync(int id, string tenantId);
    Task<IEnumerable<T>> GetAllAsync(string tenantId, bool includeDeleted = false);
    Task<IEnumerable<T>> GetActiveAsync(string tenantId);
}