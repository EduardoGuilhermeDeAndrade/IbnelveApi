namespace IbnelveApi.Application.DTOs.CategoriaTarefa;

/// <summary>
/// DTO simplificado para uso em selects
/// </summary>
public class CategoriaTarefaSelectDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Cor { get; set; }
}

