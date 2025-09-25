using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Application.Mappings;
using IbnelveApi.Application.DTOs.Categoria;

namespace IbnelveApi.Application.Services;

public class CategoriaUtensilioService : ICategoriaUtensilioService
{
    private readonly ICategoriaUtensilioRepository _repository;

    public CategoriaUtensilioService(ICategoriaUtensilioRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<CategoriaUtensilioDto>> GetByIdAsync(int id, string tenantId)
    {
        var categoria = await _repository.GetByIdAsync(id, tenantId);
        if (categoria == null)
            return ApiResponse<CategoriaUtensilioDto>.ErrorResult("Categoria não encontrada");

        return ApiResponse<CategoriaUtensilioDto>.SuccessResult(categoria.ToDto());
    }

    public async Task<ApiResponse<IEnumerable<CategoriaUtensilioDto>>> GetAllAsync(string tenantId, bool includeInactive = false)
    {
        var categorias = await _repository.GetAllAsync(tenantId, includeInactive);
        return ApiResponse<IEnumerable<CategoriaUtensilioDto>>.SuccessResult(categorias.Select(c => c.ToDto()));
    }

    public async Task<ApiResponse<CategoriaUtensilioDto>> CreateAsync(CreateCategoriaUtensilioDto createDto, string tenantId)
    {
        var categoria = new CategoriaUtensilio(createDto.Nome, createDto.Descricao, tenantId, "");
        await _repository.AddAsync(categoria);
        return ApiResponse<CategoriaUtensilioDto>.SuccessResult(categoria.ToDto());
    }

    public async Task<ApiResponse<CategoriaUtensilioDto>> UpdateAsync(int id, UpdateCategoriaUtensilioDto updateDto, string tenantId)
    {
        var categoria = await _repository.GetByIdAsync(id, tenantId);
        if (categoria == null)
            return ApiResponse<CategoriaUtensilioDto>.ErrorResult("Categoria não encontrada");

        categoria.AtualizarDados(updateDto.Nome, updateDto.Descricao);
        await _repository.UpdateAsync(categoria);
        return ApiResponse<CategoriaUtensilioDto>.SuccessResult(categoria.ToDto());
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId)
    {
        var categoria = await _repository.GetByIdAsync(id, tenantId);
        if (categoria == null)
            return ApiResponse<bool>.ErrorResult("Categoria não encontrada");

        await _repository.DeleteAsync(categoria.Id);
        return ApiResponse<bool>.SuccessResult(true);
    }

    public async Task<ApiResponse<CategoriaUtensilioDto>> GetByNomeAsync(string nome, string tenantId)
    {
        var categoria = await _repository.GetByNomeAsync(nome, tenantId);
        if (categoria == null)
            return ApiResponse<CategoriaUtensilioDto>.ErrorResult("Categoria não encontrada");

        return ApiResponse<CategoriaUtensilioDto>.SuccessResult(categoria.ToDto());
    }

    public async Task<ApiResponse<IEnumerable<CategoriaUtensilioDto>>> SearchAsync(string searchTerm, string tenantId, bool includeInactive = false)
    {
        var categorias = await _repository.SearchAsync(searchTerm, tenantId, includeInactive);
        return ApiResponse<IEnumerable<CategoriaUtensilioDto>>.SuccessResult(categorias.Select(c => c.ToDto()));
    }

    public async Task<ApiResponse<IEnumerable<CategoriaUtensilioDto>>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        bool? ativa = null,
        bool includeInactive = false,
        string orderBy = "Nome")
    {
        var categorias = await _repository.GetWithFiltersAsync(tenantId, nome, ativa, includeInactive, orderBy);
        return ApiResponse<IEnumerable<CategoriaUtensilioDto>>.SuccessResult(categorias.Select(c => c.ToDto()));
    }
}