using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IbnelveApi.Infrastructure.Repositories;

/// <summary>
/// Implementa��o do reposit�rio para LocalDeArmazenamento
/// </summary>
public class LocalDeArmazenamentoRepository : TenantRepository<LocalDeArmazenamento>, ILocalDeArmazenamentoRepository
{
    public LocalDeArmazenamentoRepository(ApplicationDbContext context) : base(context) 
    { 
    }

    public async Task<LocalDeArmazenamento?> GetByNomeAsync(string nome, string tenantId)
    {
        return await _context.LocaisDeArmazenamento
            .Where(c => c.Nome.ToLower() == nome.ToLower() &&
                       c.TenantId == tenantId &&
                       !c.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<LocalDeArmazenamento>> GetAtivasAsync(string tenantId)
    {
        return await _context.LocaisDeArmazenamento
            .Where(c => c.TenantId == tenantId &&
                       c.Ativa &&
                       !c.IsDeleted)
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<LocalDeArmazenamento>> GetAllByTenantAsync(string tenantId, bool includeDeleted = false)
    {
        var query = _context.LocaisDeArmazenamento
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
        var query = _context.LocaisDeArmazenamento
            .Where(c => c.Nome.ToLower() == nome.ToLower() &&
                       c.TenantId == tenantId &&
                       !c.IsDeleted);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<LocalDeArmazenamento>> GetAllAsync(string tenantId, bool includeDeleted = false)
    {
        var query = _context.LocaisDeArmazenamento
            .Where(t => t.TenantId == tenantId);

        if (!includeDeleted)
            query = query.Where(t => !t.IsDeleted);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<LocalDeArmazenamento>> GetActiveAsync(string tenantId)
    {
        var query = _context.LocaisDeArmazenamento
            .Where(t => t.TenantId == tenantId && t.Ativa == true);

        return await query.ToListAsync();
    }

    public async Task<bool> EstaSendoUsadaAsync(int localDeArmazenamentoId)
    {
        return await _context.Utensilios
            .Where(t => t.LocalDeArmazenamentoId == localDeArmazenamentoId && !t.IsDeleted)
            .AnyAsync();
    }

    public async Task<LocalDeArmazenamento?> GetByIdAsync(int id, string tenantId)
    {
        var query = _context.LocaisDeArmazenamento
            .Where(t => t.TenantId == tenantId && t.Id == id);

        return await query.FirstOrDefaultAsync();
    }
}