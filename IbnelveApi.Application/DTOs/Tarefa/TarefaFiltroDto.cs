using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.DTOs.Tarefa;

public class TarefaFiltroDto
{
    public StatusTarefa? Status { get; set; }
    public PrioridadeTarefa? Prioridade { get; set; }
    public int? CategoriaId { get; set; }
    public DateTime? DataVencimentoInicio { get; set; }
    public DateTime? DataVencimentoFim { get; set; }
    public bool IncludeDeleted { get; set; } = false;
    public string OrderBy { get; set; } = "CreatedAt";
    public string? SearchTerm { get; set; }
}

