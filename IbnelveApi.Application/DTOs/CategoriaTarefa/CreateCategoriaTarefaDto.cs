namespace IbnelveApi.Application.DTOs.CategoriaTarefa;

/// <summary>
/// DTO para criação de CategoriaTarefa
/// </summary>
public class CreateCategoriaTarefaDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Cor { get; set; }
}

