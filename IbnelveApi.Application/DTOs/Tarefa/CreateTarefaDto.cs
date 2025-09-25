using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.DTOs.Tarefa;

public class CreateTarefaDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;
    public DateTime? DataVencimento { get; set; }
    public int? CategoriaId { get; set; }
}

