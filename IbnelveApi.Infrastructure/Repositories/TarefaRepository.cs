using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Enums;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;

namespace IbnelveApi.Infrastructure.Repositories;

/// <summary>
/// Repositório específico para Tarefas - herda de UserOwnedRepository
/// ATUALIZADO: Aplica automaticamente filtros por UserId E TenantId
/// </summary>
public class TarefaRepository : UserOwnedRepository<Tarefa>, ITarefaRepository
{
    public TarefaRepository(ApplicationDbContext context) : base(context)
    {
    }

    // ✅ Os métodos GetByIdAsync, GetAllAsync e GetByUserAsync já são herdados da classe base
    // e aplicam automaticamente os filtros por UserId E TenantId

    public async Task<IEnumerable<Tarefa>> GetByStatusAsync(StatusTarefa status, string userId, string tenantId, bool includeDeleted = false)
    {
        return await ApplyUserAndTenantFilter(userId, tenantId, includeDeleted)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetByPrioridadeAsync(PrioridadeTarefa prioridade, string userId, string tenantId, bool includeDeleted = false)
    {
        return await ApplyUserAndTenantFilter(userId, tenantId, includeDeleted)
            .Where(t => t.Prioridade == prioridade)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetByCategoriaAsync(string categoria, string userId, string tenantId, bool includeDeleted = false)
    {
        return await ApplyUserAndTenantFilter(userId, tenantId, includeDeleted)
            .Where(t => t.Categoria == categoria)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetVencidasAsync(string userId, string tenantId, bool includeDeleted = false)
    {
        var hoje = DateTime.UtcNow.Date;

        return await ApplyUserAndTenantFilter(userId, tenantId, includeDeleted)
            .Where(t => t.DataVencimento.HasValue &&
                       t.DataVencimento.Value.Date < hoje &&
                       t.Status != StatusTarefa.Concluida)
            .OrderBy(t => t.DataVencimento)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetConcluidasAsync(string userId, string tenantId, bool includeDeleted = false)
    {
        return await ApplyUserAndTenantFilter(userId, tenantId, includeDeleted)
            .Where(t => t.Status == StatusTarefa.Concluida)
            .OrderByDescending(t => t.DataConclusao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> SearchAsync(string searchTerm, string userId, string tenantId, bool includeDeleted = false)
    {
        var termLower = searchTerm.ToLower();

        return await ApplyUserAndTenantFilter(userId, tenantId, includeDeleted)
            .Where(t => t.Titulo.ToLower().Contains(termLower) ||
                       t.Descricao.ToLower().Contains(termLower) ||
                       (t.Categoria != null && t.Categoria.ToLower().Contains(termLower)))
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> GetWithFiltersAsync(
        string userId,
        string tenantId,
        StatusTarefa? status = null,
        PrioridadeTarefa? prioridade = null,
        string? categoria = null,
        DateTime? dataVencimentoInicio = null,
        DateTime? dataVencimentoFim = null,
        bool includeDeleted = false,
        string orderBy = "CreatedAt")
    {
        var query = ApplyUserAndTenantFilter(userId, tenantId, includeDeleted);

        // Aplicar filtros opcionais
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
            "vencimento" => query.OrderBy(t => t.DataVencimento),
            "createdat" => query.OrderByDescending(t => t.CreatedAt),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        return await query.ToListAsync();
    }
}

