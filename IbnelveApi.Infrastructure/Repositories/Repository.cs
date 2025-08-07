// IbnelveApi.Infrastructure/Repositories/Repository.cs
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

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        // Global filters aplicados automaticamente
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        // Global filters aplicados automaticamente
        return await _dbSet.ToListAsync();
    }

    //public virtual async Task<IEnumerable<T>> GetAllIncludingDeletedAsync()
    //{
    //    // Ignorar filtros globais quando necessário
    //    return await _dbSet.IgnoreQueryFilters()
    //        .Where(e => _context.TenantContext.TenantId == null || e.TenantId == _context.TenantContext.TenantId)
    //        .ToListAsync();
    //}

    public virtual async Task<T> AddAsync(T entity)
    {
        // TenantId será definido automaticamente no SaveChangesAsync
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

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.ExcluirLogicamente();
            await _context.SaveChangesAsync();
        }
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        // Global filters aplicados automaticamente
        return await _dbSet.AnyAsync(e => e.Id == id);
    }
}


//using Microsoft.EntityFrameworkCore;
//using IbnelveApi.Domain.Entities;
//using IbnelveApi.Domain.Interfaces;
//using IbnelveApi.Infrastructure.Data;

//namespace IbnelveApi.Infrastructure.Repositories;

//public class Repository<T> : IRepository<T> where T : BaseEntity
//{
//    protected readonly ApplicationDbContext _context;
//    protected readonly DbSet<T> _dbSet;

//    public Repository(ApplicationDbContext context)
//    {
//        _context = context;
//        _dbSet = context.Set<T>();
//    }

//    public virtual async Task<T?> GetByIdAsync(int id, string tenantId)
//    {
//        return await _dbSet
//            .Where(e => e.Id == id && e.TenantId == tenantId)
//            .FirstOrDefaultAsync();
//    }

//    public virtual async Task<IEnumerable<T>> GetAllAsync(string tenantId, bool includeDeleted = false)
//    {
//        var query = _dbSet.Where(e => e.TenantId == tenantId);

//        if (includeDeleted)
//        {
//            query = query.IgnoreQueryFilters().Where(e => e.TenantId == tenantId);
//        }

//        return await query.ToListAsync();
//    }

//    public virtual async Task<T> AddAsync(T entity)
//    {
//        await _dbSet.AddAsync(entity);
//        await _context.SaveChangesAsync();
//        return entity;
//    }

//    public virtual async Task<T> UpdateAsync(T entity)
//    {
//        _dbSet.Update(entity);
//        await _context.SaveChangesAsync();
//        return entity;
//    }

//    public virtual async Task DeleteAsync(int id, string tenantId)
//    {
//        var entity = await GetByIdAsync(id, tenantId);
//        if (entity != null)
//        {
//            entity.ExcluirLogicamente();
//            await UpdateAsync(entity);
//        }
//    }

//    public virtual async Task<bool> ExistsAsync(int id, string tenantId)
//    {
//        return await _dbSet
//            .AnyAsync(e => e.Id == id && e.TenantId == tenantId);
//    }
//}

