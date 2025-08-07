using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;

namespace IbnelveApi.Application.Interfaces;

public interface IPessoaService
{
    Task<ApiResponse<PessoaDto>> GetByIdAsync(int id);
    Task<ApiResponse<IEnumerable<PessoaDto>>> GetAllAsync();
    Task<ApiResponse<PessoaDto>> CreateAsync(CreatePessoaDto createDto);
    Task<ApiResponse<PessoaDto>> UpdateAsync(int id, UpdatePessoaDto updateDto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
    Task<ApiResponse<PessoaDto>> GetByCpfAsync(string cpf);
    Task<ApiResponse<IEnumerable<PessoaDto>>> GetByNomeAsync(string nome);
}

