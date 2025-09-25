using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

public interface ICategoriaRepository : ITenantRepository<Categoria>
{
    Task<Categoria?> GetByNomeAsync(string nome, string tenantId);
    Task<IEnumerable<Categoria>> SearchAsync(string searchTerm, string tenantId, bool includeInactive = false);
    Task<IEnumerable<Categoria>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        bool? ativa = null,
        bool includeInactive = false,
        string orderBy = "Nome");
}