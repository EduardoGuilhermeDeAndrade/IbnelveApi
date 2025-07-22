using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Services;

public interface IProdutoService
{
    Task<ApiResponse<IEnumerable<ProdutoResponseDto>>> GetAllAsync();
    Task<ApiResponse<ProdutoResponseDto>> GetByIdAsync(int id);
    Task<ApiResponse<ProdutoResponseDto>> CreateAsync(ProdutoCreateDto produtoDto);
    Task<ApiResponse<ProdutoResponseDto>> UpdateAsync(ProdutoUpdateDto produtoDto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}

