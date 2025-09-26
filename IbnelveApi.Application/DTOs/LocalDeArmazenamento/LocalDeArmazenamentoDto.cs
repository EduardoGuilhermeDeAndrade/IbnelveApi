namespace IbnelveApi.Application.DTOs.LocalDeArmazenamento;

public class LocalDeArmazenamentoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? ContatoResponsavel { get; set; }
    public string TenantId { get; set; } = null!;
}