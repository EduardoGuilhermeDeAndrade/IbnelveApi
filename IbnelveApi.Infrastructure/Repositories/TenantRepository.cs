using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using IbnelveApi.Domain.Entities.General;

namespace IbnelveApi.Infrastructure.Repositories;

/// <summary>
/// Implementação base para repositórios de entidades do tenant
/// </summary>
public class TenantRepository<T> : Repository<T>, ITenantRepository<T> where T : TenantEntity
{
    public TenantRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Aplica filtro global por TenantId
    /// </summary>
    protected IQueryable<T> ApplyTenantFilter(string tenantId, bool includeDeleted = false)
    {
        var query = _context.Set<T>().Where(e => e.TenantId == tenantId);

        if (!includeDeleted)
        {
            query = query.Where(e => !e.IsDeleted);
        }

        return query;
    }

    public virtual async Task<T?> GetByIdAsync(int id, string tenantId)
    {
        return await ApplyTenantFilter(tenantId)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(string tenantId, bool includeDeleted = false)
    {
        return await ApplyTenantFilter(tenantId, includeDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetActiveAsync(string tenantId)
    {
        return await ApplyTenantFilter(tenantId, false)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }
}