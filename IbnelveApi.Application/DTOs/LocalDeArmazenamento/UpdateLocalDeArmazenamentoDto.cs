namespace IbnelveApi.Application.DTOs.LocalDeArmazenamento;

public class UpdateLocalDeArmazenamentoDto
{
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? ContatoResponsavel { get; set; }
}