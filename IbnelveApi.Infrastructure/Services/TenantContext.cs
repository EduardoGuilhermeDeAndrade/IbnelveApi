using IbnelveApi.Application.Interfaces;

namespace IbnelveApi.Infrastructure.Services;

public class TenantContext : ITenantContext
{
    private string? _tenantId;

    public string? TenantId => _tenantId;
    public bool HasTenant => !string.IsNullOrEmpty(_tenantId);

    public void SetTenant(string tenantId)
    {
        if (string.IsNullOrWhiteSpace(tenantId))
            throw new ArgumentException("TenantId cannot be null or empty", nameof(tenantId));

        _tenantId = tenantId;
    }
}