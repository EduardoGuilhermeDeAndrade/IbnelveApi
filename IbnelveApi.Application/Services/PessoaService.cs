using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Mappings;
using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Application.Services;

public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _pessoaRepository;

    public PessoaService(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    public async Task<ApiResponse<PessoaDto>> GetByIdAsync(int id, string tenantId)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(id, tenantId);
            
            if (pessoa == null)
                return ApiResponse<PessoaDto>.ErrorResult("Pessoa não encontrada");

            var pessoaDto = PessoaMapping.ToDto(pessoa);
            return ApiResponse<PessoaDto>.SuccessResult(pessoaDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<PessoaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<PessoaDto>>> GetAllAsync(string tenantId, bool includeDeleted = false)
    {
        try
        {
            var pessoas = await _pessoaRepository.GetAllAsync(tenantId, includeDeleted);
            var pessoasDto = PessoaMapping.ToDtoList(pessoas);
            
            return ApiResponse<IEnumerable<PessoaDto>>.SuccessResult(pessoasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<PessoaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<PessoaDto>> CreateAsync(CreatePessoaDto createDto, string tenantId)
    {
        try
        {
            // Verificar se CPF já existe
            if (await _pessoaRepository.CpfExistsAsync(createDto.CPF, tenantId))
                return ApiResponse<PessoaDto>.ErrorResult("CPF já cadastrado para este tenant");

            var pessoa = PessoaMapping.ToEntity(createDto, tenantId);
            var pessoaCriada = await _pessoaRepository.AddAsync(pessoa);
            var pessoaDto = PessoaMapping.ToDto(pessoaCriada);

            return ApiResponse<PessoaDto>.SuccessResult(pessoaDto, "Pessoa criada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<PessoaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<PessoaDto>> UpdateAsync(int id, UpdatePessoaDto updateDto, string tenantId)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(id, tenantId);
            
            if (pessoa == null)
                return ApiResponse<PessoaDto>.ErrorResult("Pessoa não encontrada");

            // Verificar se CPF já existe para outra pessoa
            if (await _pessoaRepository.CpfExistsAsync(updateDto.CPF, tenantId, id))
                return ApiResponse<PessoaDto>.ErrorResult("CPF já cadastrado para outra pessoa neste tenant");

            PessoaMapping.UpdateEntity(pessoa, updateDto);
            var pessoaAtualizada = await _pessoaRepository.UpdateAsync(pessoa);
            var pessoaDto = PessoaMapping.ToDto(pessoaAtualizada);

            return ApiResponse<PessoaDto>.SuccessResult(pessoaDto, "Pessoa atualizada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<PessoaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(id, tenantId);
            
            if (pessoa == null)
                return ApiResponse<bool>.ErrorResult("Pessoa não encontrada");

            await _pessoaRepository.DeleteAsync(id, tenantId);
            return ApiResponse<bool>.SuccessResult(true, "Pessoa excluída com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<PessoaDto>> GetByCpfAsync(string cpf, string tenantId)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetByCpfAsync(cpf, tenantId);
            
            if (pessoa == null)
                return ApiResponse<PessoaDto>.ErrorResult("Pessoa não encontrada");

            var pessoaDto = PessoaMapping.ToDto(pessoa);
            return ApiResponse<PessoaDto>.SuccessResult(pessoaDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<PessoaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<PessoaDto>>> GetByNomeAsync(string nome, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var pessoas = await _pessoaRepository.GetByNomeAsync(nome, tenantId, includeDeleted);
            var pessoasDto = PessoaMapping.ToDtoList(pessoas);
            
            return ApiResponse<IEnumerable<PessoaDto>>.SuccessResult(pessoasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<PessoaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }
}

