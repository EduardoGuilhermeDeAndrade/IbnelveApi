using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Interfaces;

/// <summary>
/// Interface do serviço de Membro
/// ATUALIZADA: Todos os métodos agora recebem tenantId para filtros globais
/// (seguindo o padrão da Tarefa, mas sem userId pois Membro é TenantEntity)
/// </summary>
public interface IMembroService
{
    // ===== MÉTODOS PRINCIPAIS COM FILTRO POR TENANT =====
    Task<ApiResponse<MembroDto>> GetByIdAsync(int id, string tenantId);
    Task<ApiResponse<IEnumerable<MembroDto>>> GetAllAsync(string tenantId, bool includeDeleted = false);
    Task<ApiResponse<MembroDto>> CreateAsync(CreateMembroDto createDto, string tenantId);
    Task<ApiResponse<MembroDto>> UpdateAsync(int id, UpdateMembroDto updateDto, string tenantId);
    Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId);

    // ===== MÉTODOS DE BUSCA COM FILTRO POR TENANT =====
    Task<ApiResponse<MembroDto>> GetByCpfAsync(string cpf, string tenantId);
    Task<ApiResponse<IEnumerable<MembroDto>>> GetByNomeAsync(string nome, string tenantId);
    Task<ApiResponse<IEnumerable<MembroDto>>> SearchAsync(string searchTerm, string tenantId, bool includeDeleted = false);

    // ===== MÉTODOS DE VALIDAÇÃO =====
    Task<ApiResponse<bool>> CpfExistsAsync(string cpf, string tenantId, int? excludeId = null);

    // ===== MÉTODOS COM FILTROS AVANÇADOS =====
    Task<ApiResponse<IEnumerable<MembroDto>>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        string? cpf = null,
        string? cidade = null,
        string? uf = null,
        bool includeDeleted = false,
        string orderBy = "Nome");
}

