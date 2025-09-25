using IbnelveApi.Application.DTOs.Membro.Endereco;

namespace IbnelveApi.Application.DTOs.Membro;

public class UpdateMembroDto
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public EnderecoDto Endereco { get; set; } = new();
}

