namespace IbnelveApi.Application.Interfaces;

public interface ITenantContext
{
    string? TenantId { get; }
    void SetTenant(string tenantId);
    bool HasTenant { get; }
}