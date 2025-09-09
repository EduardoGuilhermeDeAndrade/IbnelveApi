using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

/// <summary>
/// Interface do repositório para CategoriaTarefa
/// </summary>
public interface ICategoriaTarefaRepository : ITenantRepository<CategoriaTarefa>
{
    /// <summary>
    /// Busca categoria por nome no tenant
    /// </summary>
    Task<CategoriaTarefa?> GetByNomeAsync(string nome, string tenantId);

    /// <summary>
    /// Lista categorias ativas do tenant
    /// </summary>
    Task<IEnumerable<CategoriaTarefa>> GetAtivasAsync(string tenantId);

    /// <summary>
    /// Lista todas as categorias do tenant (ativas e inativas)
    /// </summary>
    Task<IEnumerable<CategoriaTarefa>> GetAllByTenantAsync(string tenantId, bool includeDeleted = false);

    /// <summary>
    /// Verifica se existe categoria com o nome no tenant
    /// </summary>
    Task<bool> ExisteNomeAsync(string nome, string tenantId, int? excludeId = null);

    /// <summary>
    /// Verifica se a categoria está sendo usada por alguma tarefa
    /// </summary>
    Task<bool> EstaSendoUsadaAsync(int categoriaId);

    /// <summary>
    /// Conta quantas tarefas estão usando a categoria
    /// </summary>
    Task<int> ContarTarefasAsync(int categoriaId);
}

