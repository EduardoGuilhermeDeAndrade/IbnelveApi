using IbnelveApi.Domain.Entities.General;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Domain.Entities;

/// <summary>
/// Representa um utensílio controlado pelo sistema.
/// </summary>
public class Utensilio : TenantEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Observacoes { get; set; }
    public decimal? ValorReferencia { get; set; }
    public DateTime? DataCompra { get; set; }
    public string? NumeroSerie { get; set; }
    public string? NomeFornecedor { get; set; }
    public StatusItem Situacao { get; set; }

    public int? CategoriaId { get; set; } // Torna a FK opcional
    public CategoriaUtensilio Categoria { get; set; } = null!; // Navegação
    public int? LocalDeArmazenamentoId { get; set; }
    public LocalDeArmazenamento? LocalDeArmazenamento { get; set; }

    // Propriedade de navegação para fotos
    public ICollection<FotoUtensilio> Fotos { get; set; } = new List<FotoUtensilio>();

    public Utensilio() { }

    public Utensilio(string nome, string? descricao, string? observacoes, decimal? valorReferencia, DateTime? dataCompra, string? numeroSerie, string? nomeFornecedor, StatusItem situacao, int categoriaId, string tenantId)
    {
        Nome = nome;
        Descricao = descricao;
        Observacoes = observacoes;
        ValorReferencia = valorReferencia;
        DataCompra = dataCompra;
        NumeroSerie = numeroSerie;
        NomeFornecedor = nomeFornecedor;
        Situacao = situacao;
        CategoriaId = categoriaId;
        TenantId = tenantId;
    }

    public void AtualizarDados(
        string nome,
        string? descricao,
        string? observacoes,
        decimal? valorReferencia,
        DateTime? dataCompra,
        string? numeroSerie,
        string? nomeFornecedor,
        StatusItem situacao,
        int categoriaId)
    {
        Nome = nome;
        Descricao = descricao;
        Observacoes = observacoes;
        ValorReferencia = valorReferencia;
        DataCompra = dataCompra;
        NumeroSerie = numeroSerie;
        NomeFornecedor = nomeFornecedor;
        Situacao = situacao;
        CategoriaId = categoriaId;
        UpdatedAt = DateTime.UtcNow;
    }
}