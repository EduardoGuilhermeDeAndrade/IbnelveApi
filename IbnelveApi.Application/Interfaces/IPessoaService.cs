using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Interfaces;

public interface IPessoaService
{
    Task<ApiResponse<PessoaDto>> GetByIdAsync(int id, string tenantId);
    Task<ApiResponse<IEnumerable<PessoaDto>>> GetAllAsync(string tenantId, bool includeDeleted = false);
    Task<ApiResponse<PessoaDto>> CreateAsync(CreatePessoaDto createDto, string tenantId);
    Task<ApiResponse<PessoaDto>> UpdateAsync(int id, UpdatePessoaDto updateDto, string tenantId);
    Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId);
    Task<ApiResponse<PessoaDto>> GetByCpfAsync(string cpf, string tenantId);
    Task<ApiResponse<IEnumerable<PessoaDto>>> GetByNomeAsync(string nome, string tenantId, bool includeDeleted = false);
}

