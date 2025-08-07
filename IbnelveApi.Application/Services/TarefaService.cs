using IbnelveApi.Application.Common;
using IbnelveApi.Application.DTOs;
using IbnelveApi.Application.Interfaces;
using IbnelveApi.Application.Mappings;
using IbnelveApi.Domain.Enums;
using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Application.Services;

public class TarefaService : ITarefaService
{
    private readonly ITarefaRepository _tarefaRepository;

    public TarefaService(ITarefaRepository tarefaRepository)
    {
        _tarefaRepository = tarefaRepository;
    }

    public async Task<ApiResponse<TarefaDto>> GetByIdAsync(int id, string tenantId)
    {
        try
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id);
            
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

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetAllAsync(string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetAllAsync();
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);
            
            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetWithFiltersAsync(TarefaFiltroDto filtro, string tenantId)
    {
        try
        {
            IEnumerable<Domain.Entities.Tarefa> tarefas;

            if (!string.IsNullOrEmpty(filtro.SearchTerm))
            {
                tarefas = await _tarefaRepository.SearchAsync(filtro.SearchTerm, tenantId, filtro.IncludeDeleted);
            }
            else
            {
                tarefas = await _tarefaRepository.GetWithFiltersAsync(
                    tenantId,
                    filtro.Status,
                    filtro.Prioridade,
                    filtro.Categoria,
                    filtro.DataVencimentoInicio,
                    filtro.DataVencimentoFim,
                    filtro.IncludeDeleted,
                    filtro.OrderBy);
            }

            var tarefasDto = TarefaMapping.ToDtoList(tarefas);
            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> SearchAsync(string searchTerm, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.SearchAsync(searchTerm, tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);
            
            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<TarefaDto>> CreateAsync(CreateTarefaDto createDto, string tenantId)
    {
        try
        {
            var tarefa = TarefaMapping.ToEntity(createDto, tenantId);
            var tarefaCriada = await _tarefaRepository.AddAsync(tarefa);
            var tarefaDto = TarefaMapping.ToDto(tarefaCriada);

            return ApiResponse<TarefaDto>.SuccessResult(tarefaDto, "Tarefa criada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<TarefaDto>> UpdateAsync(int id, UpdateTarefaDto updateDto, string tenantId)
    {
        try
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id);
            
            if (tarefa == null)
                return ApiResponse<TarefaDto>.ErrorResult("Tarefa não encontrada");

            TarefaMapping.UpdateEntity(tarefa, updateDto);
            var tarefaAtualizada = await _tarefaRepository.UpdateAsync(tarefa);
            var tarefaDto = TarefaMapping.ToDto(tarefaAtualizada);

            return ApiResponse<TarefaDto>.SuccessResult(tarefaDto, "Tarefa atualizada com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<TarefaDto>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<TarefaDto>> UpdateStatusAsync(int id, StatusTarefa status, string tenantId)
    {
        try
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id);
            
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

    public async Task<ApiResponse<TarefaDto>> MarcarComoConcluidaAsync(int id, string tenantId)
    {
        return await UpdateStatusAsync(id, StatusTarefa.Concluida, tenantId);
    }

    public async Task<ApiResponse<TarefaDto>> MarcarComoPendenteAsync(int id, string tenantId)
    {
        return await UpdateStatusAsync(id, StatusTarefa.Pendente, tenantId);
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, string tenantId)
    {
        try
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id);
            
            if (tarefa == null)
                return ApiResponse<bool>.ErrorResult("Tarefa não encontrada");

            await _tarefaRepository.DeleteAsync(id);
            return ApiResponse<bool>.SuccessResult(true, "Tarefa excluída com sucesso");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetByStatusAsync(StatusTarefa status, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetByStatusAsync(status, tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);
            
            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetByPrioridadeAsync(PrioridadeTarefa prioridade, string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetByPrioridadeAsync(prioridade, tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);
            
            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetVencidasAsync(string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetVencidasAsync(tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);
            
            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<TarefaDto>>> GetConcluidasAsync(string tenantId, bool includeDeleted = false)
    {
        try
        {
            var tarefas = await _tarefaRepository.GetConcluidasAsync(tenantId, includeDeleted);
            var tarefasDto = TarefaMapping.ToDtoList(tarefas);
            
            return ApiResponse<IEnumerable<TarefaDto>>.SuccessResult(tarefasDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TarefaDto>>.ErrorResult("Erro interno do servidor", ex.Message);
        }
    }
}

