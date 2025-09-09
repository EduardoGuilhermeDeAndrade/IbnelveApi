namespace IbnelveApi.Application.DTOs;

/// <summary>
/// DTO para leitura de CategoriaTarefa
/// </summary>
public class CategoriaTarefaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Cor { get; set; }
    public bool Ativa { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int QuantidadeTarefas { get; set; } // Quantidade de tarefas usando esta categoria
}

/// <summary>
/// DTO para criação de CategoriaTarefa
/// </summary>
public class CreateCategoriaTarefaDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Cor { get; set; }
}

/// <summary>
/// DTO para atualização de CategoriaTarefa
/// </summary>
public class UpdateCategoriaTarefaDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Cor { get; set; }
}

/// <summary>
/// DTO simplificado para uso em selects
/// </summary>
public class CategoriaTarefaSelectDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Cor { get; set; }
}

