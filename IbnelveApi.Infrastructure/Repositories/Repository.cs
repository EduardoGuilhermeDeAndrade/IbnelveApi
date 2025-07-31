using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;

namespace IbnelveApi.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id, string tenantId)
    {
        return await _dbSet
            .Where(e => e.Id == id && e.TenantId == tenantId)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(string tenantId, bool includeDeleted = false)
    {
        var query = _dbSet.Where(e => e.TenantId == tenantId);

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters().Where(e => e.TenantId == tenantId);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(int id, string tenantId)
    {
        var entity = await GetByIdAsync(id, tenantId);
        if (entity != null)
        {
            entity.ExcluirLogicamente();
            await UpdateAsync(entity);
        }
    }

    public virtual async Task<bool> ExistsAsync(int id, string tenantId)
    {
        return await _dbSet
            .AnyAsync(e => e.Id == id && e.TenantId == tenantId);
    }
}

