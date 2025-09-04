namespace IbnelveApi.Domain.Entities;

/// <summary>
/// Entidade base para dados específicos do usuário
/// (ex: tarefas pessoais, notas, lembretes, etc.)
/// Possui TenantId E UserId
/// </summary>
public abstract class UserOwnedEntity : TenantEntity
{
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Verifica se a entidade pertence ao usuário especificado
    /// </summary>
    public bool BelongsToUser(string userId)
    {
        return UserId == userId;
    }

    /// <summary>
    /// Verifica se a entidade pertence ao tenant e usuário especificados
    /// </summary>
    public bool BelongsToUserAndTenant(string userId, string tenantId)
    {
        return UserId == userId && TenantId == tenantId;
    }
}

