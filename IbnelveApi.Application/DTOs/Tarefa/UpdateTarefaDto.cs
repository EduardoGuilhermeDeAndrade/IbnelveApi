using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.DTOs.Tarefa;

public class UpdateTarefaDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; }
    public DateTime? DataVencimento { get; set; }
    public int? CategoriaId { get; set; }
}

