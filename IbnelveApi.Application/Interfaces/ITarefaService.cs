using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.Tarefa;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.Interfaces;

/// <summary>
/// Interface do serviço de Tarefas
/// ATUALIZADA: Todos os métodos agora recebem userId E tenantId para filtros globais
/// </summary>
public interface ITarefaService
{
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetAllAsync(string userId, string tenantId, bool includeDeleted = false);
    Task<ApiResponse<TarefaDto>> GetByIdAsync(int id, string userId, string tenantId);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetWithFiltersAsync(TarefaFiltroDto filtro, string userId, string tenantId);
    Task<ApiResponse<IEnumerable<TarefaDto>>> SearchAsync(string searchTerm, string userId, string tenantId, bool includeDeleted = false);
    Task<ApiResponse<TarefaDto>> CreateAsync(CreateTarefaDto createDto, string userId, string tenantId);
    Task<ApiResponse<TarefaDto>> UpdateAsync(int id, UpdateTarefaDto updateDto, string userId, string tenantId);
    Task<ApiResponse<TarefaDto>> UpdateStatusAsync(int id, StatusTarefa status, string userId, string tenantId);
    Task<ApiResponse<TarefaDto>> MarcarComoConcluidaAsync(int id, string userId, string tenantId);
    Task<ApiResponse<TarefaDto>> MarcarComoPendenteAsync(int id, string userId, string tenantId);
    Task<ApiResponse<bool>> DeleteAsync(int id, string userId, string tenantId);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetByStatusAsync(StatusTarefa status, string userId, string tenantId, bool includeDeleted = false);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetByPrioridadeAsync(PrioridadeTarefa prioridade, string userId, string tenantId, bool includeDeleted = false);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetVencidasAsync(string userId, string tenantId, bool includeDeleted = false);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetConcluidasAsync(string userId, string tenantId, bool includeDeleted = false);
}

