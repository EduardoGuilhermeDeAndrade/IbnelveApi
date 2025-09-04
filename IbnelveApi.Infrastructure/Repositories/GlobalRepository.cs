using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;

namespace IbnelveApi.Infrastructure.Repositories;

/// <summary>
/// Implementação base para repositórios de entidades globais
/// </summary>
public class GlobalRepository<T> : Repository<T>, IGlobalRepository<T> where T : GlobalEntity
{
    public GlobalRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Aplica filtro apenas por IsDeleted (sem TenantId)
    /// </summary>
    protected IQueryable<T> ApplyGlobalFilter(bool includeDeleted = false)
    {
        var query = _context.Set<T>().AsQueryable();

        if (!includeDeleted)
        {
            query = query.Where(e => !e.IsDeleted);
        }

        return query;
    }

    public override async Task<T?> GetByIdAsync(int id)
    {
        return await ApplyGlobalFilter()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public override async Task<IEnumerable<T>> GetAllAsync()
    {
        return await GetAllAsync(false);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false)
    {
        return await ApplyGlobalFilter(includeDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetActiveAsync()
    {
        return await ApplyGlobalFilter(false)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }
}