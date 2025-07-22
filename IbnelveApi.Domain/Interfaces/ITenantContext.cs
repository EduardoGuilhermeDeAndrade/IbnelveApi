namespace IbnelveApi.Domain.Interfaces;

public interface ITenantContext
{
    string TenantId { get; }
    void SetTenantId(string tenantId);
}

