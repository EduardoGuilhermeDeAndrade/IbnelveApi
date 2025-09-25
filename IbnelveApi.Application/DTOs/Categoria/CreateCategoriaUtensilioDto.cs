namespace IbnelveApi.Application.DTOs.Categoria;

public class CreateCategoriaUtensilioDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Cor { get; set; }
}