namespace IbnelveApi.Application.DTOs.LocalDeArmazenamento;

public class CreateLocalDeArmazenamentoDto
{
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? ContatoResponsavel { get; set; }
}