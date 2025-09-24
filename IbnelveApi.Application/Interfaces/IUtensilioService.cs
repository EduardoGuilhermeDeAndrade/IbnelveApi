using IbnelveApi.Application.Dtos.Utensilio;

namespace IbnelveApi.Application.Interfaces;

/// <summary>
/// Interface para o serviço de utensílios.
/// </summary>
public interface IUtensilioService
{
    Task<IEnumerable<UtensilioDto>> GetAllAsync(string tenantId);
    Task<UtensilioDto?> GetByIdAsync(int id, string tenantId);
    Task<UtensilioDto> CreateAsync(CreateUtensilioDto dto, string tenantId);
    Task<bool> UpdateAsync(UpdateUtensilioDto dto, string tenantId);
    Task<bool> DeleteAsync(int id, string tenantId);
}