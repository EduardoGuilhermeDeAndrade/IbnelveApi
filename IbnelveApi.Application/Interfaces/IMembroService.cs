using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Interfaces;

/// <summary>
/// Interface do servi�o de Membro
/// ATUALIZADA: Todos os m�todos agora recebem tenantId para filtros globais
/// (seguindo o padr�o da Tarefa, mas sem userId pois Membro � TenantEntity)
/// </summary>
public interface IMembroService
{
    // ===== M�TODOS PRINCIPAIS COM FILTRO POR TENANT =====
    Task<ApiResponse<MembroDto>> GetByIdAsync(int id, string tenantId);
    Task<ApiResponse<IEnumerable<MembroDto>>> GetAllAsync(string tenantId, bool includeDeleted = false);
    Task<ApiResponse<MembroDto>> CreateAsync(CreateMembroDto createDto, string tenantId);
    Task<ApiResponse<MembroDto>> UpdateAsync(int id, UpdateMembroDto updateDto, string tenantId);
    Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId);

    // ===== M�TODOS DE BUSCA COM FILTRO POR TENANT =====
    Task<ApiResponse<MembroDto>> GetByCpfAsync(string cpf, string tenantId);
    Task<ApiResponse<IEnumerable<MembroDto>>> GetByNomeAsync(string nome, string tenantId);
    Task<ApiResponse<IEnumerable<MembroDto>>> SearchAsync(string searchTerm, string tenantId, bool includeDeleted = false);

    // ===== M�TODOS DE VALIDA��O =====
    Task<ApiResponse<bool>> CpfExistsAsync(string cpf, string tenantId, int? excludeId = null);

    // ===== M�TODOS COM FILTROS AVAN�ADOS =====
    Task<ApiResponse<IEnumerable<MembroDto>>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        string? cpf = null,
        string? cidade = null,
        string? uf = null,
        bool includeDeleted = false,
        string orderBy = "Nome");
}

