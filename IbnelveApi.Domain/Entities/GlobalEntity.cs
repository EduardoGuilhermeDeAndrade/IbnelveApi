namespace IbnelveApi.Domain.Entities;

/// <summary>
/// Entidade base para dados globais compartilhados entre TODOS os tenants
/// (ex: País, Estado, Cidade, Moeda, etc.)
/// NÃO possui TenantId - são dados de referência globais
/// </summary>
public abstract class GlobalEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual void ExcluirLogicamente()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}

