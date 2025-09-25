using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IbnelveApi.Infrastructure.Repositories;

public class CategoriaRepository : TenantRepository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Categoria?> GetByNomeAsync(string nome, string tenantId)
    {
        return await ApplyTenantFilter(tenantId)
            .Where(c => c.Nome == nome)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Categoria>> SearchAsync(string searchTerm, string tenantId, bool includeInactive = false)
    {
        var termLower = searchTerm.ToLower();
        var query = ApplyTenantFilter(tenantId, includeInactive)
            .Where(c => c.Nome.ToLower().Contains(termLower) || (c.Descricao != null && c.Descricao.ToLower().Contains(termLower)));

        return await query.OrderBy(c => c.Nome).ToListAsync();
    }

    public async Task<IEnumerable<Categoria>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        bool? ativa = null,
        bool includeInactive = false,
        string orderBy = "Nome")
    {
        var query = ApplyTenantFilter(tenantId, includeInactive);

        if (!string.IsNullOrEmpty(nome))
            query = query.Where(c => c.Nome.Contains(nome));

        if (ativa.HasValue)
            query = query.Where(c => c.Ativa == ativa.Value);

        query = orderBy.ToLower() switch
        {
            "nome" => query.OrderBy(c => c.Nome),
            "ativa" => query.OrderByDescending(c => c.Ativa),
            _ => query.OrderBy(c => c.Nome)
        };

        return await query.ToListAsync();
    }
}