using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.FotoUtensilio;

public class FotoUtensilioService : IFotoUtensilioService
{
    private readonly IFotoUtensilioRepository _FotoUtensilioRepository;

    public FotoUtensilioService(IFotoUtensilioRepository fotoUtensilioRepository)
    {
        _FotoUtensilioRepository = fotoUtensilioRepository;
    }

    public async Task<ApiResponse<List<FotoUtensilioDto>>> GetAllAsync(string tenantId)
    {
        var entities = await _FotoUtensilioRepository.GetAllAsync(tenantId, false); 
        var dtos = entities.Select(x => x.ToDto()).ToList();
        return ApiResponse<List<FotoUtensilioDto>>.SuccessResult(dtos);
    }

    public async Task<ApiResponse<FotoUtensilioDto>> GetByIdAsync(int id, string tenantId)
    {
        var entity = await _FotoUtensilioRepository.GetByIdAsync(id, tenantId);
        if (entity == null || entity.IsDeleted)
            return ApiResponse<FotoUtensilioDto>.ErrorResult("Registro não encontrado");
        return ApiResponse<FotoUtensilioDto>.SuccessResult(entity.ToDto());
    }

    public async Task<ApiResponse<FotoUtensilioDto>> CreateAsync(CreateFotoUtensilioDto dto, int utensilioId, string tenantId)
    {
        var entity = dto.ToEntity(utensilioId, tenantId);
        await _FotoUtensilioRepository.AddAsync(entity);
        return ApiResponse<FotoUtensilioDto>.SuccessResult(entity.ToDto());
    }

    public async Task<ApiResponse<FotoUtensilioDto>> UpdateAsync(int id, UpdateFotoUtensilioDto dto, string tenantId)
    {
        var entity = await _FotoUtensilioRepository.GetByIdAsync(id, tenantId);
        if (entity == null || entity.IsDeleted)
            return ApiResponse<FotoUtensilioDto>.ErrorResult("Registro não encontrado");

        entity.ArquivoPath = dto.ArquivoPath;
        entity.Descricao = dto.Descricao;
        entity.IsPrincipal = dto.IsPrincipal;
        entity.UpdatedAt = DateTime.UtcNow;

        await _FotoUtensilioRepository.UpdateAsync(entity);
        return ApiResponse<FotoUtensilioDto>.SuccessResult(entity.ToDto());
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId)
    {
        var entity = await _FotoUtensilioRepository.GetByIdAsync(id, tenantId);
        if (entity == null || entity.IsDeleted)
            return ApiResponse<bool>.ErrorResult("Registro não encontrado");

        entity.IsDeleted = true;
        await _FotoUtensilioRepository.UpdateAsync(entity);
        return ApiResponse<bool>.SuccessResult(true);
    }
}