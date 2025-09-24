namespace IbnelveApi.Application.Dtos.Utensilio;

/// <summary>
/// DTO para criação de utensílio.
/// </summary>
public class CreateUtensilioDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Observacoes { get; set; }
    public decimal? ValorReferencia { get; set; }
    public DateTime? DataCompra { get; set; }
    public string? NumeroSerie { get; set; }
    public string? NomeFornecedor { get; set; }
    public int Situacao { get; set; }
}