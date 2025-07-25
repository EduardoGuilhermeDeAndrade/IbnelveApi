namespace IbnelveApi.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

