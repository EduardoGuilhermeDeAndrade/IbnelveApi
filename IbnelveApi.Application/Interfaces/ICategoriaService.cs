using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.Categoria;

namespace IbnelveApi.Application.Interfaces;

public interface ICategoriaService
{
    Task<ApiResponse<CategoriaDto>> GetByIdAsync(int id, string tenantId);
    Task<ApiResponse<IEnumerable<CategoriaDto>>> GetAllAsync(string tenantId, bool includeInactive = false);
    Task<ApiResponse<CategoriaDto>> CreateAsync(CreateCategoriaDto createDto, string tenantId);
    Task<ApiResponse<CategoriaDto>> UpdateAsync(int id, UpdateCategoriaDto updateDto, string tenantId);
    Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId);

    Task<ApiResponse<CategoriaDto>> GetByNomeAsync(string nome, string tenantId);
    Task<ApiResponse<IEnumerable<CategoriaDto>>> SearchAsync(string searchTerm, string tenantId, bool includeInactive = false);
    Task<ApiResponse<IEnumerable<CategoriaDto>>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        bool? ativa = null,
        bool includeInactive = false,
        string orderBy = "Nome");
}