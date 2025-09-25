using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.Categoria;

namespace IbnelveApi.Application.Interfaces;

public interface ICategoriaUtensilioService
{
    Task<ApiResponse<CategoriaUtensilioDto>> GetByIdAsync(int id, string tenantId);
    Task<ApiResponse<IEnumerable<CategoriaUtensilioDto>>> GetAllAsync(string tenantId, bool includeInactive = false);
    Task<ApiResponse<CategoriaUtensilioDto>> CreateAsync(CreateCategoriaUtensilioDto createDto, string tenantId);
    Task<ApiResponse<CategoriaUtensilioDto>> UpdateAsync(int id, UpdateCategoriaUtensilioDto updateDto, string tenantId);
    Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId);

    Task<ApiResponse<CategoriaUtensilioDto>> GetByNomeAsync(string nome, string tenantId);
    Task<ApiResponse<IEnumerable<CategoriaUtensilioDto>>> SearchAsync(string searchTerm, string tenantId, bool includeInactive = false);
    Task<ApiResponse<IEnumerable<CategoriaUtensilioDto>>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        bool? ativa = null,
        bool includeInactive = false,
        string orderBy = "Nome");
}