using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.Interfaces;

public interface ITarefaService
{
    Task<ApiResponse<TarefaDto>> GetByIdAsync(int id, string tenantId);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetAllAsync(string tenantId, bool includeDeleted = false);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetWithFiltersAsync(TarefaFiltroDto filtro, string tenantId);
    Task<ApiResponse<IEnumerable<TarefaDto>>> SearchAsync(string searchTerm, string tenantId, bool includeDeleted = false);
    Task<ApiResponse<TarefaDto>> CreateAsync(CreateTarefaDto createDto, string tenantId);
    Task<ApiResponse<TarefaDto>> UpdateAsync(int id, UpdateTarefaDto updateDto, string tenantId);
    Task<ApiResponse<TarefaDto>> UpdateStatusAsync(int id, StatusTarefa status, string tenantId);
    Task<ApiResponse<TarefaDto>> MarcarComoConcluidaAsync(int id, string tenantId);
    Task<ApiResponse<TarefaDto>> MarcarComoPendenteAsync(int id, string tenantId);
    Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetByStatusAsync(StatusTarefa status, string tenantId, bool includeDeleted = false);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetByPrioridadeAsync(PrioridadeTarefa prioridade, string tenantId, bool includeDeleted = false);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetVencidasAsync(string tenantId, bool includeDeleted = false);
    Task<ApiResponse<IEnumerable<TarefaDto>>> GetConcluidasAsync(string tenantId, bool includeDeleted = false);
}

