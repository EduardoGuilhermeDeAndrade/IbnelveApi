using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Domain.Interfaces;

public interface ITarefaRepository : IRepository<Tarefa>
{
    Task<IEnumerable<Tarefa>> GetByStatusAsync(StatusTarefa status, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetByPrioridadeAsync(PrioridadeTarefa prioridade, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetByCategoriaAsync(string categoria, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetByTituloAsync(string titulo, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetVencidasAsync(string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetConcluidasAsync(string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> SearchAsync(string searchTerm, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetWithFiltersAsync(
        string tenantId,
        StatusTarefa? status = null,
        PrioridadeTarefa? prioridade = null,
        string? categoria = null,
        DateTime? dataVencimentoInicio = null,
        DateTime? dataVencimentoFim = null,
        bool includeDeleted = false,
        string orderBy = "CreatedAt");
}

