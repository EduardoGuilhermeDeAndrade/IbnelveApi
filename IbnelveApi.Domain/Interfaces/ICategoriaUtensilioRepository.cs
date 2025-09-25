using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

public interface ICategoriaUtensilioRepository : ITenantRepository<CategoriaUtensilio>
{
    Task<CategoriaUtensilio?> GetByNomeAsync(string nome, string tenantId);
    Task<IEnumerable<CategoriaUtensilio>> SearchAsync(string searchTerm, string tenantId, bool includeInactive = false);
    Task<IEnumerable<CategoriaUtensilio>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        bool? ativa = null,
        bool includeInactive = false,
        string orderBy = "Nome");
}