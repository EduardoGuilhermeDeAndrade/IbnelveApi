namespace IbnelveApi.Domain.Entities.General;

/// <summary>
/// Entidade base para dados compartilhados dentro do tenant
/// (ex: categorias, configurações do tenant, departamentos, etc.)
/// Possui TenantId mas NÃO possui UserId
/// </summary>
public abstract class TenantEntity : GlobalEntity
{
    public string TenantId { get; set; } = string.Empty;

    /// <summary>
    /// Verifica se a entidade pertence ao tenant especificado
    /// </summary>
    public bool BelongsToTenant(string tenantId)
    {
        return TenantId == tenantId;
    }
}

