namespace IbnelveApi.Application.DTOs;

public class ProdutoUpdateDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
}

