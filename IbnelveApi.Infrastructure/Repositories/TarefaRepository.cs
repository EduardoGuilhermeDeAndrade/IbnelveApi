using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Enums;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;

namespace IbnelveApi.Infrastructure.Repositories;

public class TarefaRepository : Repository<Tarefa>, ITarefaRepository
{
    public TarefaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tarefa>> GetByStatusAsync(StatusTarefa status, string tenantId, bool includeDeleted = false)
    {
        var query = _dbSet.Where(t => t.Status == status && t.TenantId == tenantId);

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters().Where(t => t.TenantId == tenantId && t.Status == status);
        }

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetByPrioridadeAsync(PrioridadeTarefa prioridade, string tenantId, bool includeDeleted = false)
    {
        var query = _dbSet.Where(t => t.Prioridade == prioridade && t.TenantId == tenantId);

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters().Where(t => t.TenantId == tenantId && t.Prioridade == prioridade);
        }

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetByCategoriaAsync(string categoria, string tenantId, bool includeDeleted = false)
    {
        var query = _dbSet.Where(t => t.Categoria == categoria && t.TenantId == tenantId);

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters().Where(t => t.TenantId == tenantId && t.Categoria == categoria);
        }

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetByTituloAsync(string titulo, string tenantId, bool includeDeleted = false)
    {
        var query = _dbSet.Where(t => t.Titulo.Contains(titulo) && t.TenantId == tenantId);

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters().Where(t => t.TenantId == tenantId && t.Titulo.Contains(titulo));
        }

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetVencidasAsync(string tenantId, bool includeDeleted = false)
    {
        var hoje = DateTime.UtcNow.Date;
        var query = _dbSet.Where(t => 
            t.TenantId == tenantId && 
            t.DataVencimento.HasValue && 
            t.DataVencimento.Value.Date < hoje && 
            t.Status != StatusTarefa.Concluida);

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters().Where(t => 
                t.TenantId == tenantId && 
                t.DataVencimento.HasValue && 
                t.DataVencimento.Value.Date < hoje && 
                t.Status != StatusTarefa.Concluida);
        }

        return await query.OrderBy(t => t.DataVencimento).ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetConcluidasAsync(string tenantId, bool includeDeleted = false)
    {
        var query = _dbSet.Where(t => t.Status == StatusTarefa.Concluida && t.TenantId == tenantId);

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters().Where(t => t.TenantId == tenantId && t.Status == StatusTarefa.Concluida);
        }

        return await query.OrderByDescending(t => t.DataConclusao).ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> SearchAsync(string searchTerm, string tenantId, bool includeDeleted = false)
    {
        var query = _dbSet.Where(t => 
            t.TenantId == tenantId && 
            (t.Titulo.Contains(searchTerm) || t.Descricao.Contains(searchTerm)));

        if (includeDeleted)
        {
            query = query.IgnoreQueryFilters().Where(t => 
                t.TenantId == tenantId && 
                (t.Titulo.Contains(searchTerm) || t.Descricao.Contains(searchTerm)));
        }

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetWithFiltersAsync(
        string tenantId,
        StatusTarefa? status = null,
        PrioridadeTarefa? prioridade = null,
        string? categoria = null,
        DateTime? dataVencimentoInicio = null,
        DateTime? dataVencimentoFim = null,
        bool includeDeleted = false,
        string orderBy = "CreatedAt")
    {
        var query = includeDeleted 
            ? _dbSet.IgnoreQueryFilters().Where(t => t.TenantId == tenantId)
            : _dbSet.Where(t => t.TenantId == tenantId);

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (prioridade.HasValue)
            query = query.Where(t => t.Prioridade == prioridade.Value);

        if (!string.IsNullOrEmpty(categoria))
            query = query.Where(t => t.Categoria == categoria);

        if (dataVencimentoInicio.HasValue)
            query = query.Where(t => t.DataVencimento >= dataVencimentoInicio.Value);

        if (dataVencimentoFim.HasValue)
            query = query.Where(t => t.DataVencimento <= dataVencimentoFim.Value);

        // Aplicar ordenação
        query = orderBy.ToLower() switch
        {
            "titulo" => query.OrderBy(t => t.Titulo),
            "status" => query.OrderBy(t => t.Status),
            "prioridade" => query.OrderByDescending(t => t.Prioridade),
            "datavencimento" => query.OrderBy(t => t.DataVencimento),
            "dataconclusao" => query.OrderByDescending(t => t.DataConclusao),
            "updatedat" => query.OrderByDescending(t => t.UpdatedAt),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        return await query.ToListAsync();
    }
}

