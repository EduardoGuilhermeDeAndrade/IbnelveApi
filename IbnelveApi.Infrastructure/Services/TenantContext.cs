using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Infrastructure.Services;

public class TenantContext : ITenantContext
{
    private string _tenantId = string.Empty;

    public string TenantId => _tenantId;

    public void SetTenantId(string tenantId)
    {
        _tenantId = tenantId ?? string.Empty;
    }
}

