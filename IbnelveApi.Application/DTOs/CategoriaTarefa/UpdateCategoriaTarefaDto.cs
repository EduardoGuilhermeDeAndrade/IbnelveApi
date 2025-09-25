namespace IbnelveApi.Application.DTOs.CategoriaTarefa;

/// <summary>
/// DTO para atualização de CategoriaTarefa
/// </summary>
public class UpdateCategoriaTarefaDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Cor { get; set; }
}

