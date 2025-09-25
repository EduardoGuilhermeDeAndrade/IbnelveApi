using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.CategoriaTarefa;

namespace IbnelveApi.Application.Interfaces;

/// <summary>
/// Interface do service para CategoriaTarefa
/// </summary>
public interface ICategoriaTarefaService
{
    /// <summary>
    /// Lista todas as categorias do tenant
    /// </summary>
    Task<ApiResponse<IEnumerable<CategoriaTarefaDto>>> GetAllAsync(string tenantId, bool includeDeleted = false);

    /// <summary>
    /// Lista apenas categorias ativas para uso em selects
    /// </summary>
    Task<ApiResponse<IEnumerable<CategoriaTarefaSelectDto>>> GetAtivasForSelectAsync(string tenantId);

    /// <summary>
    /// Busca categoria por ID
    /// </summary>
    Task<ApiResponse<CategoriaTarefaDto>> GetByIdAsync(int id, string tenantId);

    /// <summary>
    /// Cria nova categoria
    /// </summary>
    Task<ApiResponse<CategoriaTarefaDto>> CreateAsync(CreateCategoriaTarefaDto dto, string tenantId, string userId);

    /// <summary>
    /// Atualiza categoria existente
    /// </summary>
    Task<ApiResponse<CategoriaTarefaDto>> UpdateAsync(int id, UpdateCategoriaTarefaDto dto, string tenantId);

    /// <summary>
    /// Ativa categoria
    /// </summary>
    Task<ApiResponse<bool>> AtivarAsync(int id, string tenantId);

    /// <summary>
    /// Desativa categoria
    /// </summary>
    Task<ApiResponse<bool>> DesativarAsync(int id, string tenantId);

    /// <summary>
    /// Exclui categoria (soft delete)
    /// </summary>
    Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId);

    /// <summary>
    /// Verifica se nome já existe no tenant
    /// </summary>
    Task<bool> NomeJaExisteAsync(string nome, string tenantId, int? excludeId = null);
}

