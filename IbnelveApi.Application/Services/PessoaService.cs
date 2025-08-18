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

    public async Task<ApiResponse<PessoaDto>> GetByIdAsync(int id)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(id);
            
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

    public async Task<ApiResponse<IEnumerable<PessoaDto>>> GetAllAsync()
    {
        try
        {
            var pessoas = await _pessoaRepository.GetAllAsync();
            var pessoasDto = PessoaMapping.ToDtoList(pessoas);
            
            return ApiResponse<IEnumerable<PessoaDto>>.SuccessResult(pessoasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<PessoaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<PessoaDto>> CreateAsync(CreatePessoaDto createDto)
    {
        try
        {
            var pessoa = PessoaMapping.ToEntity(createDto);
            var createdPessoa = await _pessoaRepository.AddAsync(pessoa);
            return ApiResponse<PessoaDto>.SuccessResult(PessoaMapping.ToDto(createdPessoa), "Sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<PessoaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<PessoaDto>> UpdateAsync(int id, UpdatePessoaDto updateDto)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(id);
            
            if (pessoa == null)
                return ApiResponse<PessoaDto>.ErrorResult("Pessoa não encontrada");

            // Verificar se CPF já existe para outra pessoa
            // Claro que existe pois é alteração.
            if (!await _pessoaRepository.CpfExistsAsync(updateDto.CPF))
                return ApiResponse<PessoaDto>.ErrorResult("Usuário ainda não cadastrado neste tenant");

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

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(id);
            
            if (pessoa == null)
                return ApiResponse<bool>.ErrorResult("Pessoa não encontrada");

            await _pessoaRepository.DeleteAsync(id);
            return ApiResponse<bool>.SuccessResult(true, "Pessoa excluída com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<PessoaDto>> GetByCpfAsync(string cpf)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetByCpfAsync(cpf);
            
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

    public async Task<ApiResponse<IEnumerable<PessoaDto>>> GetByNomeAsync(string nome)
    {
        try
        {
            var pessoas = await _pessoaRepository.GetByNomeAsync(nome);
            var pessoasDto = PessoaMapping.ToDtoList(pessoas);
            
            return ApiResponse<IEnumerable<PessoaDto>>.SuccessResult(pessoasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<PessoaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }
}

