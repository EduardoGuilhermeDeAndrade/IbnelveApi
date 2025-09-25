using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs.Membro;
using IbnelveApi.Application.Extensions;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Mappings;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Application.Services;

/// <summary>
/// Serviço de Membro
/// ATUALIZADO: Todos os métodos agora usam tenantId para filtros globais
/// (seguindo o padrão da Tarefa, mas sem userId pois Membro é TenantEntity)
/// </summary>
public class MembroService : IMembroService
{
    private readonly IMembroRepository _membroRepository;

    public MembroService(IMembroRepository membroRepository)
    {
        _membroRepository = membroRepository;
    }

    // ===== MÉTODOS PRINCIPAIS COM FILTRO POR TENANT =====

    public async Task<ApiResponse<MembroDto>> GetByIdAsync(int id, string tenantId)
    {
        try
        {
            var membro = await _membroRepository.GetByIdAsync(id, tenantId);

            if (membro == null)
                return ApiResponse<MembroDto>.ErrorResult("Membro não encontrada");

            var membroDto = MembroMapping.ToDto(membro);
            return ApiResponse<MembroDto>.SuccessResult(membroDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<MembroDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<MembroDto>>> GetAllAsync(string tenantId, bool includeDeleted = false)
    {
        try
        {
            var membros = await _membroRepository.GetAllAsync(tenantId, includeDeleted);
            var membrosDto = MembroMapping.ToDtoList(membros);

            return ApiResponse<IEnumerable<MembroDto>>.SuccessResult(membrosDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<MembroDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<MembroDto>> CreateAsync(CreateMembroDto createDto, string tenantId)
    {
        try
        {
            // Limpar caracteres especiais
            createDto.CPF = createDto.CPF.RemoveSpecialCharacters();
            createDto.Telefone = createDto.Telefone.RemoveSpecialCharacters();
            createDto.Endereco.CEP = createDto.Endereco.CEP.RemoveSpecialCharacters();

            // Verificar se CPF já existe no tenant
            if (await _membroRepository.CpfExistsAsync(createDto.CPF, tenantId))
                return ApiResponse<MembroDto>.ErrorResult("CPF já cadastrado neste tenant");

            // Criar membro com tenantId
            var endereco = MembroMapping.ToEnderecoEntity(createDto.Endereco);
            var membro = new Membro(createDto.Nome, createDto.CPF, createDto.Telefone, endereco, tenantId);

            var createdMembro = await _membroRepository.AddAsync(membro);
            var membroDto = MembroMapping.ToDto(createdMembro);

            return ApiResponse<MembroDto>.SuccessResult(membroDto, "Membro criada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<MembroDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<MembroDto>> UpdateAsync(int id, UpdateMembroDto updateDto, string tenantId)
    {
        try
        {
            // Limpar caracteres especiais
            updateDto.CPF = updateDto.CPF.RemoveSpecialCharacters();
            updateDto.Telefone = updateDto.Telefone.RemoveSpecialCharacters();
            updateDto.Endereco.CEP = updateDto.Endereco.CEP.RemoveSpecialCharacters();

            var membro = await _membroRepository.GetByIdAsync(id, tenantId);

            if (membro == null)
                return ApiResponse<MembroDto>.ErrorResult("Membro não encontrada");

            // Verificar se CPF já existe (excluindo a própria membro)
            if (await _membroRepository.CpfExistsAsync(updateDto.CPF, tenantId, id))
                return ApiResponse<MembroDto>.ErrorResult("CPF já cadastrado para outra membro neste tenant");

            // Atualizar dados
            var endereco = MembroMapping.ToEnderecoEntity(updateDto.Endereco);
            membro.AtualizarDados(updateDto.Nome, updateDto.CPF, updateDto.Telefone, endereco);

            var membroAtualizada = await _membroRepository.UpdateAsync(membro);
            var membroDto = MembroMapping.ToDto(membroAtualizada);

            return ApiResponse<MembroDto>.SuccessResult(membroDto, "Membro atualizada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<MembroDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId)
    {
        try
        {
            var membro = await _membroRepository.GetByIdAsync(id, tenantId);

            if (membro == null)
                return ApiResponse<bool>.ErrorResult("Membro não encontrada");

            // Soft delete
            membro.ExcluirLogicamente();
            await _membroRepository.UpdateAsync(membro);

            return ApiResponse<bool>.SuccessResult(true, "Membro excluída com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    // ===== MÉTODOS DE BUSCA COM FILTRO POR TENANT =====

    public async Task<ApiResponse<MembroDto>> GetByCpfAsync(string cpf, string tenantId)
    {
        try
        {
            cpf = cpf.RemoveSpecialCharacters();
            var membro = await _membroRepository.GetByCpfAsync(cpf, tenantId);

            if (membro == null)
                return ApiResponse<MembroDto>.ErrorResult("Membro não encontrada");

            var membroDto = MembroMapping.ToDto(membro);
            return ApiResponse<MembroDto>.SuccessResult(membroDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<MembroDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<MembroDto>>> GetByNomeAsync(string nome, string tenantId)
    {
        try
        {
            var membros = await _membroRepository.GetByNomeAsync(nome, tenantId);
            var membrosDto = MembroMapping.ToDtoList(membros);

            return ApiResponse<IEnumerable<MembroDto>>.SuccessResult(membrosDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<MembroDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<MembroDto>>> SearchAsync(string searchTerm, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var membros = await _membroRepository.SearchAsync(searchTerm, tenantId, includeDeleted);
            var membrosDto = MembroMapping.ToDtoList(membros);

            return ApiResponse<IEnumerable<MembroDto>>.SuccessResult(membrosDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<MembroDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    // ===== MÉTODOS DE VALIDAÇÃO =====

    public async Task<ApiResponse<bool>> CpfExistsAsync(string cpf, string tenantId, int? excludeId = null)
    {
        try
        {
            cpf = cpf.RemoveSpecialCharacters();
            var exists = await _membroRepository.CpfExistsAsync(cpf, tenantId, excludeId);

            return ApiResponse<bool>.SuccessResult(exists);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    // ===== MÉTODOS COM FILTROS AVANÇADOS =====

    public async Task<ApiResponse<IEnumerable<MembroDto>>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        string? cpf = null,
        string? cidade = null,
        string? uf = null,
        bool includeDeleted = false,
        string orderBy = "Nome")
    {
        try
        {
            // Limpar CPF se fornecido
            if (!string.IsNullOrEmpty(cpf))
                cpf = cpf.RemoveSpecialCharacters();

            var membros = await _membroRepository.GetWithFiltersAsync(
                tenantId, nome, cpf, cidade, uf, includeDeleted, orderBy);

            var membrosDto = MembroMapping.ToDtoList(membros);
            return ApiResponse<IEnumerable<MembroDto>>.SuccessResult(membrosDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<MembroDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    
}

