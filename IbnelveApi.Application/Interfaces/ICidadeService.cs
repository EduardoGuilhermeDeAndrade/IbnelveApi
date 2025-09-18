using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Common;

namespace IbnelveApi.Application.Interfaces;

public interface ICidadeService
{
    Task<ApiResponse<IEnumerable<CidadeDto>>> GetAllAsync(bool includeDeleted = false);
    Task<ApiResponse<CidadeDto>> GetByIdAsync(int id);
    Task<ApiResponse<CidadeDto>> CreateAsync(CreateCidadeDto dto);
    Task<ApiResponse<CidadeDto>> UpdateAsync(int id, UpdateCidadeDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}
