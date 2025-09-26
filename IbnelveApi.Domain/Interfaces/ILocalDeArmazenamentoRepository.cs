using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

public interface ILocalDeArmazenamentoRepository : ITenantRepository<LocalDeArmazenamento>
{
    Task<LocalDeArmazenamento?> GetByNomeAsync(string nome, string tenantId);
    Task<IEnumerable<LocalDeArmazenamento>> GetAtivasAsync(string tenantId);
    Task<IEnumerable<LocalDeArmazenamento>> GetAllByTenantAsync(string tenantId, bool includeDeleted = false);
    Task<bool> ExisteNomeAsync(string nome, string tenantId, int? excludeId = null);
    Task<IEnumerable<LocalDeArmazenamento>> GetAllAsync(string tenantId, bool includeDeleted = false);
    Task<IEnumerable<LocalDeArmazenamento>> GetActiveAsync(string tenantId);
    Task<bool> EstaSendoUsadaAsync(int categoriaId);
    Task<LocalDeArmazenamento?> GetByIdAsync(int id, string tenantId);
}
    