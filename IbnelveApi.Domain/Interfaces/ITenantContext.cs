namespace IbnelveApi.Domain.Interfaces;

public interface ITenantContext
{
    string? TenantId { get; }
    void SetTenant(string tenantId);
    bool HasTenant { get; }
}