using IbnelveApi.Application.DTOs.CategoriaTarefa;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.DTOs.Tarefa;

public class TarefaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public StatusTarefa Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; }
    public string PrioridadeDescricao { get; set; } = string.Empty;
    public DateTime? DataVencimento { get; set; }
    public DateTime? DataConclusao { get; set; }
    public int? CategoriaId { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool EstaVencida { get; set; }
    public bool EstaConcluida { get; set; }
    public CategoriaTarefaSelectDto? Categoria { get; set; }
}

