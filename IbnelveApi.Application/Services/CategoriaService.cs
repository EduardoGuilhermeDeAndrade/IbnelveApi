using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Application.Mappings;
using IbnelveApi.Application.DTOs.Categoria;

namespace IbnelveApi.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<CategoriaDto>> GetByIdAsync(int id, string tenantId)
    {
        var categoria = await _repository.GetByIdAsync(id, tenantId);
        if (categoria == null)
            return ApiResponse<CategoriaDto>.ErrorResult("Categoria não encontrada");

        return ApiResponse<CategoriaDto>.SuccessResult(categoria.ToDto());
    }

    public async Task<ApiResponse<IEnumerable<CategoriaDto>>> GetAllAsync(string tenantId, bool includeInactive = false)
    {
        var categorias = await _repository.GetAllAsync(tenantId, includeInactive);
        return ApiResponse<IEnumerable<CategoriaDto>>.SuccessResult(categorias.Select(c => c.ToDto()));
    }

    public async Task<ApiResponse<CategoriaDto>> CreateAsync(CreateCategoriaDto createDto, string tenantId)
    {
        var categoria = new Categoria(createDto.Nome, createDto.Descricao, tenantId, "");
        await _repository.AddAsync(categoria);
        return ApiResponse<CategoriaDto>.SuccessResult(categoria.ToDto());
    }

    public async Task<ApiResponse<CategoriaDto>> UpdateAsync(int id, UpdateCategoriaDto updateDto, string tenantId)
    {
        var categoria = await _repository.GetByIdAsync(id, tenantId);
        if (categoria == null)
            return ApiResponse<CategoriaDto>.ErrorResult("Categoria não encontrada");

        categoria.AtualizarDados(updateDto.Nome, updateDto.Descricao);
        await _repository.UpdateAsync(categoria);
        return ApiResponse<CategoriaDto>.SuccessResult(categoria.ToDto());
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId)
    {
        var categoria = await _repository.GetByIdAsync(id, tenantId);
        if (categoria == null)
            return ApiResponse<bool>.ErrorResult("Categoria não encontrada");

        await _repository.DeleteAsync(categoria.Id);
        return ApiResponse<bool>.SuccessResult(true);
    }

    public async Task<ApiResponse<CategoriaDto>> GetByNomeAsync(string nome, string tenantId)
    {
        var categoria = await _repository.GetByNomeAsync(nome, tenantId);
        if (categoria == null)
            return ApiResponse<CategoriaDto>.ErrorResult("Categoria não encontrada");

        return ApiResponse<CategoriaDto>.SuccessResult(categoria.ToDto());
    }

    public async Task<ApiResponse<IEnumerable<CategoriaDto>>> SearchAsync(string searchTerm, string tenantId, bool includeInactive = false)
    {
        var categorias = await _repository.SearchAsync(searchTerm, tenantId, includeInactive);
        return ApiResponse<IEnumerable<CategoriaDto>>.SuccessResult(categorias.Select(c => c.ToDto()));
    }

    public async Task<ApiResponse<IEnumerable<CategoriaDto>>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        bool? ativa = null,
        bool includeInactive = false,
        string orderBy = "Nome")
    {
        var categorias = await _repository.GetWithFiltersAsync(tenantId, nome, ativa, includeInactive, orderBy);
        return ApiResponse<IEnumerable<CategoriaDto>>.SuccessResult(categorias.Select(c => c.ToDto()));
    }
}