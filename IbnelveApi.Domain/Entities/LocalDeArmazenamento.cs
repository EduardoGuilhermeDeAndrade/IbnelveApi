using IbnelveApi.Domain.Entities.General;

namespace IbnelveApi.Domain.Entities;

public class LocalDeArmazenamento : TenantEntity
{
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? ContatoResponsavel { get; set; }
    public bool Ativa { get; set; } = true;
    public ICollection<Utensilio> Utensilios { get; set; } = new List<Utensilio>();

    public LocalDeArmazenamento() { }

    public LocalDeArmazenamento(string nome, string? descricao, string? contatoResponsavel, string tenantId)
    {
        Nome = nome;
        Descricao = descricao;
        ContatoResponsavel = contatoResponsavel;
        TenantId = tenantId;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void AtualizarDados(string nome, string? descricao, string? contatoResponsavel)
    {
        Nome = nome;
        Descricao = descricao;
        ContatoResponsavel = contatoResponsavel;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ExcluirLogicamente()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}