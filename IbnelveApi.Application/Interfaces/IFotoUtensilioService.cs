using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.FotoUtensilio;

public interface IFotoUtensilioService
{
    Task<ApiResponse<List<FotoUtensilioDto>>> GetAllAsync(string tenantId);
    Task<ApiResponse<FotoUtensilioDto>> GetByIdAsync(int id, string tenantId);
    Task<ApiResponse<FotoUtensilioDto>> CreateAsync(CreateFotoUtensilioDto dto, int utensilioId, string tenantId);
    Task<ApiResponse<FotoUtensilioDto>> UpdateAsync(int id, UpdateFotoUtensilioDto dto, string tenantId);
    Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId);
}