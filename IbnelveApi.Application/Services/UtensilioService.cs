using IbnelveApi.Application.Dtos.Utensilio;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Mappings;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Application.Services;

public class UtensilioService: IUtensilioService
{
    private readonly IUtensilioRepository _utensilioRepository;

    public UtensilioService(IUtensilioRepository utensilioRepository)
    {
        _utensilioRepository = utensilioRepository;
    }

    public async Task<IEnumerable<UtensilioDto>> GetAllAsync(string tenantId)
    {
        var entities = await _utensilioRepository.GetAllAsync(tenantId);
        return entities.Select(x => x.ToDto());
    }

    public async Task<UtensilioDto?> GetByIdAsync(int id, string tenantId)
    {
        var entity = await _utensilioRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantId != tenantId) return null;
        return entity.ToDto();
    }

    public async Task<UtensilioDto> CreateAsync(CreateUtensilioDto dto, string tenantId)
    {
        var entity = dto.ToEntity(tenantId);
        await _utensilioRepository.AddAsync(entity);
        return entity.ToDto();
    }

    public async Task<bool> UpdateAsync(UpdateUtensilioDto dto, string tenantId)
    {
        var entity = await _utensilioRepository.GetByIdAsync(dto.Id);
        if (entity == null || entity.TenantId != tenantId) return false;

        entity.AtualizarDados(
            dto.Nome,
            dto.Descricao,
            dto.Observacoes,
            dto.ValorReferencia,
            dto.DataCompra,
            dto.NumeroSerie,
            dto.NomeFornecedor,
            (Domain.Enums.StatusItem)dto.Situacao,
            dto.CategoriaId
        );
        await _utensilioRepository.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string tenantId)
    {
        var entity = await _utensilioRepository.GetByIdAsync(id, tenantId);
        if (entity == null || entity.TenantId != tenantId) return false;
        await _utensilioRepository.DeleteAsync(entity.Id);
        return true;
    }
}