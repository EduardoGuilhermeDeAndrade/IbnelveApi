//namespace IbnelveApi.Domain.Entities;

//public abstract class BaseEntity
//{
//    public int Id { get; set; }
//    public string TenantId { get; set; } = string.Empty;
//    public bool IsDeleted { get; set; } = false;
//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//    public DateTime? UpdatedAt { get; set; }

//    public virtual void ExcluirLogicamente()
//    {
//        IsDeleted = true;
//        UpdatedAt = DateTime.UtcNow;
//    }
//}

namespace IbnelveApi.Domain.Entities;

/// <summary>
/// COMPATIBILIDADE: Mantém BaseEntity como alias para TenantEntity
/// para não quebrar código existente
/// </summary>
public abstract class BaseEntity : TenantEntity
{
    // Esta classe existe apenas para compatibilidade
    // Novas entidades devem usar TenantEntity ou UserOwnedEntity diretamente
}