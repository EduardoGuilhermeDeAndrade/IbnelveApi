namespace IbnelveApi.Application.DTOs;

public class MembroDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public EnderecoDto Endereco { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateMembroDto
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public EnderecoDto Endereco { get; set; } = new();
}

public class UpdateMembroDto
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public EnderecoDto Endereco { get; set; } = new();
}

