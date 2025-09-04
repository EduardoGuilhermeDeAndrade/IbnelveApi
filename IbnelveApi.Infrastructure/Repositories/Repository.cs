using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;

namespace IbnelveApi.Infrastructure.Repositories;

/// <summary>
/// Implementação base do repositório genérico
/// ATUALIZADA: Constraint adequada para GlobalEntity (base de todas as entidades)
/// </summary>
public class Repository<T> : IRepository<T> where T : GlobalEntity
{
    protected readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _context.Set<T>().AnyAsync(e => e.Id == id);
    }
}





