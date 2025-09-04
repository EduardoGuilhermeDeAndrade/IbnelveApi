using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Mappings;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Enums;
using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Application.Services;

/// <summary>
/// Serviço de Tarefas
/// ATUALIZADO: Todos os métodos agora usam userId E tenantId para filtros globais
/// </summary>
public class TarefaService : ITarefaService
{
    private readonly ITarefaRepository _tarefaRepository;

    public TarefaService(ITarefaRepository tarefaRepository)
    {
        _tarefaRepository = tarefaRepository;
    }

    public async Task<ApiResponse<TarefaDto>> GetByIdAsync(int id, string userId, string tenantId)
    {
        try
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id, userId, tenantId);

            if (tarefa == null)
                return ApiResponse<TarefaDto>.ErrorResult("Tarefa não encontrada");

            var tarefaDto = TarefaMapping.ToDto(tarefa);
            return ApiResponse<TarefaDto>.SuccessResult(tarefaDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetAllAsync(string userId, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetAllAsync(userId, tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);

            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetWithFiltersAsync(TarefaFiltroDto filtro, string userId, string tenantId)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetWithFiltersAsync(
                userId,
                tenantId,
                filtro.Status,
                filtro.Prioridade,
                filtro.Categoria,
                filtro.DataVencimentoInicio,
                filtro.DataVencimentoFim,
                filtro.IncludeDeleted,
                filtro.OrderBy ?? "CreatedAt");

            var tarefasDto = TarefaMapping.ToDtoList(tarefas);
            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> SearchAsync(string searchTerm, string userId, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.SearchAsync(searchTerm, userId, tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);

            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<TarefaDto>> CreateAsync(CreateTarefaDto createDto, string userId, string tenantId)
    {
        try
        {
            var tarefa = new Tarefa(
                createDto.Titulo,
                createDto.Descricao,
                userId,
                tenantId,
                createDto.Prioridade,
                createDto.DataVencimento,
                createDto.Categoria
            );

            var tarefaCriada = await _tarefaRepository.AddAsync(tarefa);
            var tarefaDto = TarefaMapping.ToDto(tarefaCriada);

            return ApiResponse<TarefaDto>.SuccessResult(tarefaDto, "Tarefa criada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<TarefaDto>> UpdateAsync(int id, UpdateTarefaDto updateDto, string userId, string tenantId)
    {
        try
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id, userId, tenantId);

            if (tarefa == null)
                return ApiResponse<TarefaDto>.ErrorResult("Tarefa não encontrada");

            tarefa.AtualizarDados(
                updateDto.Titulo,
                updateDto.Descricao,
                updateDto.Prioridade,
                updateDto.DataVencimento,
                updateDto.Categoria
            );

            var tarefaAtualizada = await _tarefaRepository.UpdateAsync(tarefa);
            var tarefaDto = TarefaMapping.ToDto(tarefaAtualizada);

            return ApiResponse<TarefaDto>.SuccessResult(tarefaDto, "Tarefa atualizada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<TarefaDto>> UpdateStatusAsync(int id, StatusTarefa status, string userId, string tenantId)
    {
        try
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id, userId, tenantId);

            if (tarefa == null)
                return ApiResponse<TarefaDto>.ErrorResult("Tarefa não encontrada");

            tarefa.AlterarStatus(status);

            var tarefaAtualizada = await _tarefaRepository.UpdateAsync(tarefa);
            var tarefaDto = TarefaMapping.ToDto(tarefaAtualizada);

            return ApiResponse<TarefaDto>.SuccessResult(tarefaDto, "Status da tarefa atualizado com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<TarefaDto>> MarcarComoConcluidaAsync(int id, string userId, string tenantId)
    {
        return await UpdateStatusAsync(id, StatusTarefa.Concluida, userId, tenantId);
    }

    public async Task<ApiResponse<TarefaDto>> MarcarComoPendenteAsync(int id, string userId, string tenantId)
    {
        return await UpdateStatusAsync(id, StatusTarefa.Pendente, userId, tenantId);
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, string userId, string tenantId)
    {
        try
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id, userId, tenantId);

            if (tarefa == null)
                return ApiResponse<bool>.ErrorResult("Tarefa não encontrada");

            tarefa.ExcluirLogicamente();
            await _tarefaRepository.UpdateAsync(tarefa);

            return ApiResponse<bool>.SuccessResult(true, "Tarefa excluída com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetByStatusAsync(StatusTarefa status, string userId, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetByStatusAsync(status, userId, tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);

            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetByPrioridadeAsync(PrioridadeTarefa prioridade, string userId, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetByPrioridadeAsync(prioridade, userId, tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);

            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetVencidasAsync(string userId, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetVencidasAsync(userId, tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);

            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetConcluidasAsync(string userId, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetConcluidasAsync(userId, tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);

            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }
}

