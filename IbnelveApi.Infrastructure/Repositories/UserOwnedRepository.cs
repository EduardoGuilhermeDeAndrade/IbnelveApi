using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using IbnelveApi.Domain.Entities.General;

namespace IbnelveApi.Infrastructure.Repositories;


/// <summary>
/// Implementação base para repositórios de entidades específicas do usuário
/// </summary>
public class UserOwnedRepository<T> : Repository<T>, IUserOwnedRepository<T> where T : UserOwnedEntity
{
    public UserOwnedRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Aplica filtros globais por UserId E TenantId
    /// </summary>
    protected IQueryable<T> ApplyUserAndTenantFilter(string userId, string tenantId, bool includeDeleted = false)
    {
        var query = _context.Set<T>()
            .Where(e => e.UserId == userId && e.TenantId == tenantId);

        if (!includeDeleted)
        {
            query = query.Where(e => !e.IsDeleted);
        }

        return query;
    }

    public virtual async Task<T?> GetByIdAsync(int id, string userId, string tenantId)
    {
        return await ApplyUserAndTenantFilter(userId, tenantId)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(string userId, string tenantId, bool includeDeleted = false)
    {
        return await ApplyUserAndTenantFilter(userId, tenantId, includeDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetByUserAsync(string userId, string tenantId, bool includeDeleted = false)
    {
        return await GetAllAsync(userId, tenantId, includeDeleted);
    }
}

