using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Domain.Interfaces;

/// <summary>
/// Interface para repositório de Tarefas (entidade específica do usuário)
/// ATUALIZADA: Agora herda de IUserOwnedRepository e todos os métodos recebem userId E tenantId
/// </summary>
public interface ITarefaRepository : IUserOwnedRepository<Tarefa>
{
    Task<IEnumerable<Tarefa>> GetByStatusAsync(StatusTarefa status, string userId, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetByPrioridadeAsync(PrioridadeTarefa prioridade, string userId, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetByCategoriaAsync(string categoria, string userId, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetVencidasAsync(string userId, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetConcluidasAsync(string userId, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> SearchAsync(string searchTerm, string userId, string tenantId, bool includeDeleted = false);
    Task<IEnumerable<Tarefa>> GetWithFiltersAsync(
        string userId,
        string tenantId,
        StatusTarefa? status = null,
        PrioridadeTarefa? prioridade = null,
        string? categoria = null,
        DateTime? dataVencimentoInicio = null,
        DateTime? dataVencimentoFim = null,
        bool includeDeleted = false,
        string orderBy = "CreatedAt");
}

