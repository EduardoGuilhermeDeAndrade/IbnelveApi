using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

public interface IMembroRepository : ITenantRepository<Membro>
{
    Task<Membro?> GetByCpfAsync(string cpf, string tenantId);
    Task<IEnumerable<Membro>> GetByNomeAsync(string nome, string tenantId);
    Task<bool> CpfExistsAsync(string cpf, string tenantId, int? excludeId = null);


    Task<IEnumerable<Membro>> SearchAsync(string searchTerm, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Membro>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        string? cpf = null,
        string? cidade = null,
        string? uf = null,
        bool includeDeleted = false,
        string orderBy = "Nome");
}

