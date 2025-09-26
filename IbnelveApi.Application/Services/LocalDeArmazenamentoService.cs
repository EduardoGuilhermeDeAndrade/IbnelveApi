using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.LocalDeArmazenamento;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Application.Services;

public class LocalDeArmazenamentoService : ILocalDeArmazenamentoService
{
    private readonly ILocalDeArmazenamentoRepository _repository;

    public LocalDeArmazenamentoService(ILocalDeArmazenamentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<LocalDeArmazenamentoDto>> GetByIdAsync(int id, string tenantId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.TenantId != tenantId || entity.IsDeleted)
            return ApiResponse<LocalDeArmazenamentoDto>.ErrorResult("Local de armazenamento não encontrado.");

        return ApiResponse<LocalDeArmazenamentoDto>.SuccessResult(ToDto(entity));
    }

    public async Task<ApiResponse<IEnumerable<LocalDeArmazenamentoDto>>> GetAllAsync(string tenantId, bool includeDeleted = false)
    {
        var entities = await _repository.GetAllAsync(tenantId, includeDeleted);
        return ApiResponse<IEnumerable<LocalDeArmazenamentoDto>>.SuccessResult(entities.Select(ToDto));
    }

    public async Task<ApiResponse<LocalDeArmazenamentoDto>> CreateAsync(CreateLocalDeArmazenamentoDto dto, string tenantId)
    {
        var entity = new LocalDeArmazenamento(dto.Nome, dto.Descricao, dto.ContatoResponsavel, tenantId);
        await _repository.AddAsync(entity);
        return ApiResponse<LocalDeArmazenamentoDto>.SuccessResult(ToDto(entity));
    }

    public async Task<ApiResponse<LocalDeArmazenamentoDto>> UpdateAsync(int id, UpdateLocalDeArmazenamentoDto dto, string tenantId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.TenantId != tenantId || entity.IsDeleted)
            return ApiResponse<LocalDeArmazenamentoDto>.ErrorResult("Local de armazenamento não encontrado.");

        entity.AtualizarDados(dto.Nome, dto.Descricao, dto.ContatoResponsavel);
        await _repository.UpdateAsync(entity);
        return ApiResponse<LocalDeArmazenamentoDto>.SuccessResult(ToDto(entity));
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null || entity.TenantId != tenantId || entity.IsDeleted)
            return ApiResponse<bool>.ErrorResult("Local de armazenamento não encontrado.");

        entity.ExcluirLogicamente();
        await _repository.UpdateAsync(entity);
        return ApiResponse<bool>.SuccessResult(true);
    }

    private static LocalDeArmazenamentoDto ToDto(LocalDeArmazenamento entity) =>
        new()
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Descricao = entity.Descricao,
            ContatoResponsavel = entity.ContatoResponsavel,
            TenantId = entity.TenantId
        };
}