namespace IbnelveApi.Application.DTOs;

public class CidadeDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string UF { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public string? CodigoIBGE { get; set; }
    public bool Capital { get; set; }
    public int? EstadoId { get; set; }
    public string? EstadoNome { get; set; }
}
