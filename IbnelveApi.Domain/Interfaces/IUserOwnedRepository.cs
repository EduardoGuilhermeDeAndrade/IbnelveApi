using IbnelveApi.Domain.Entities.General;

namespace IbnelveApi.Domain.Interfaces;





/// <summary>
/// Interface base para repositórios de entidades específicas do usuário
/// </summary>
public interface IUserOwnedRepository<T> : IRepository<T> where T : UserOwnedEntity
{
    Task<T?> GetByIdAsync(int id, string userId, string tenantId);
    Task<IEnumerable<T>> GetAllAsync(string userId, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<T>> GetByUserAsync(string userId, string tenantId, bool includeDeleted = false);
}
