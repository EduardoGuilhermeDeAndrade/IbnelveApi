using IbnelveApi.Application.DTOs;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.Mappings;

public static class TarefaMapping
{
    public static TarefaDto ToDto(Tarefa tarefa)
    {
        return new TarefaDto
        {
            Id = tarefa.Id,
            Titulo = tarefa.Titulo,
            Descricao = tarefa.Descricao,
            Status = tarefa.Status,
            StatusDescricao = GetStatusDescricao(tarefa.Status),
            Prioridade = tarefa.Prioridade,
            PrioridadeDescricao = GetPrioridadeDescricao(tarefa.Prioridade),
            DataVencimento = tarefa.DataVencimento,
            DataConclusao = tarefa.DataConclusao,
            TenantId = tarefa.TenantId,
            CreatedAt = tarefa.CreatedAt,
            UpdatedAt = tarefa.UpdatedAt,
            EstaVencida = tarefa.EstaVencida(),
            EstaConcluida = tarefa.EstaConcluida(),
            CategoriaId = tarefa.CategoriaId,
            Categoria = tarefa.Categoria?.ToSelectDto(),

        };
    }

    public static Tarefa ToEntity(CreateTarefaDto createDto, string tenantId, string userId)
    {
        return new Tarefa(
    createDto.Titulo,
    createDto.Descricao,
    createDto.Prioridade,
    createDto.DataVencimento,
    createDto.CategoriaId, 
            tenantId,
            userId
);
    }

    public static void UpdateEntity(Tarefa tarefa, UpdateTarefaDto updateDto)
    {
        tarefa.AtualizarDados(
    updateDto.Titulo,
    updateDto.Descricao,
    updateDto.Prioridade,
    updateDto.DataVencimento,
    updateDto.CategoriaId 
);
    }

    public static IEnumerable<TarefaDto> ToDtoList(IEnumerable<Tarefa> tarefas)
    {
        return tarefas.Select(ToDto);
    }

    private static string GetStatusDescricao(StatusTarefa status)
    {
        return status switch
        {
            StatusTarefa.Pendente => "Pendente",
            StatusTarefa.EmAndamento => "Em Andamento",
            StatusTarefa.Concluida => "Concluída",
            StatusTarefa.Cancelada => "Cancelada",
            _ => "Desconhecido"
        };
    }

    private static string GetPrioridadeDescricao(PrioridadeTarefa prioridade)
    {
        return prioridade switch
        {
            PrioridadeTarefa.Baixa => "Baixa",
            PrioridadeTarefa.Media => "Média",
            PrioridadeTarefa.Alta => "Alta",
            PrioridadeTarefa.Critica => "Crítica",
            _ => "Desconhecida"
        };
    }
}

