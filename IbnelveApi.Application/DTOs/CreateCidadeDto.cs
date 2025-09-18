namespace IbnelveApi.Application.DTOs;

public class CreateCidadeDto
{
    public string Nome { get; set; } = string.Empty;
    public string UF { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public string? CodigoIBGE { get; set; }
    public bool Capital { get; set; } = false;
    public int? EstadoId { get; set; }
}
