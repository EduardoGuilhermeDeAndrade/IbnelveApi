using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IbnelveApi.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para CategoriaTarefa
/// </summary>
public class CategoriaTarefaRepository : Repository<CategoriaTarefa>, ICategoriaTarefaRepository
{
    public CategoriaTarefaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<CategoriaTarefa?> GetByNomeAsync(string nome, string tenantId)
    {
        return await _context.CategoriaTarefas
            .Where(c => c.Nome.ToLower() == nome.ToLower() && 
                       c.TenantId == tenantId && 
                       !c.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CategoriaTarefa>> GetAtivasAsync(string tenantId)
    {
        return await _context.CategoriaTarefas
            .Where(c => c.TenantId == tenantId && 
                       c.Ativa && 
                       !c.IsDeleted)
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<CategoriaTarefa>> GetAllByTenantAsync(string tenantId, bool includeDeleted = false)
    {
        var query = _context.CategoriaTarefas
            .Where(c => c.TenantId == tenantId);

        if (!includeDeleted)
        {
            query = query.Where(c => !c.IsDeleted);
        }

        return await query
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<bool> ExisteNomeAsync(string nome, string tenantId, int? excludeId = null)
    {
        var query = _context.CategoriaTarefas
            .Where(c => c.Nome.ToLower() == nome.ToLower() && 
                       c.TenantId == tenantId && 
                       !c.IsDeleted);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public Task<CategoriaTarefa?> GetByIdAsync(int id, string tenantId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CategoriaTarefa>> GetAllAsync(string tenantId, bool includeDeleted = false)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CategoriaTarefa>> GetActiveAsync(string tenantId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EstaSendoUsadaAsync(int categoriaId)
    {
        return await _context.Tarefas
            .Where(t => t.CategoriaId == categoriaId && !t.IsDeleted)
            .AnyAsync();
    }

    public async Task<int> ContarTarefasAsync(int categoriaId)
    {
        return await _context.Tarefas
            .Where(t => t.CategoriaId == categoriaId && !t.IsDeleted)
            .CountAsync();
    }
}

