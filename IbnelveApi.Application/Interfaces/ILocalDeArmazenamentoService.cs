using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.LocalDeArmazenamento;

namespace IbnelveApi.Application.Interfaces;

public interface ILocalDeArmazenamentoService
{
    Task<ApiResponse<LocalDeArmazenamentoDto>> GetByIdAsync(int id, string tenantId);
    Task<ApiResponse<IEnumerable<LocalDeArmazenamentoDto>>> GetAllAsync(string tenantId, bool includeDeleted = false);
    Task<ApiResponse<LocalDeArmazenamentoDto>> CreateAsync(CreateLocalDeArmazenamentoDto dto, string tenantId);
    Task<ApiResponse<LocalDeArmazenamentoDto>> UpdateAsync(int id, UpdateLocalDeArmazenamentoDto dto, string tenantId);
    Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId);
}