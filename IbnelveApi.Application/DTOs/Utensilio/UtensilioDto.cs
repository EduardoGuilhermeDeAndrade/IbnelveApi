namespace IbnelveApi.Application.Dtos.Utensilio;

/// <summary>
/// DTO de leitura de utensílio.
/// </summary>
public class UtensilioDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Observacoes { get; set; }
    public decimal? ValorReferencia { get; set; }
    public DateTime? DataCompra { get; set; }
    public string? NumeroSerie { get; set; }
    public string? NomeFornecedor { get; set; }
    public int Situacao { get; set; }
    public int CategoriaId { get; set; }
    public string TenantId { get; set; } = string.Empty;
}