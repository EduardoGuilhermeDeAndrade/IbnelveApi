// IbnelveApi.Api/Middlewares/TenantMiddleware.cs
using IbnelveApi.Application.Interfaces;
using System.Security.Claims;

namespace IbnelveApi.Api.Middlewares;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantMiddleware> _logger;

    public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        try
        {
            // Extrair tenant do JWT
            var tenantId = ExtractTenantFromToken(context);

            if (!string.IsNullOrEmpty(tenantId))
            {
                tenantContext.SetTenant(tenantId);
                _logger.LogDebug("Tenant {TenantId} definido no contexto", tenantId);
            }
            else if (RequiresTenant(context))
            {
                _logger.LogWarning("Endpoint requer tenant mas nenhum foi encontrado no token");
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Tenant ID is required");
                return;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar tenant no middleware");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Internal server error");
            return;
        }

        await _next(context);
    }

    private string? ExtractTenantFromToken(HttpContext context)
    {
        // Verificar se usuário está autenticado
        if (!context.User.Identity?.IsAuthenticated == true)
            return null;

        // Extrair claim do tenant
        var tenantClaim = context.User.FindFirst("tenantId") ??
                         context.User.FindFirst("tenant_id") ??
                         context.User.FindFirst(ClaimTypes.GroupSid);

        return tenantClaim?.Value;
    }

    private bool RequiresTenant(HttpContext context)
    {
        // Endpoints que não requerem tenant (auth, health check, etc.)
        var path = context.Request.Path.Value?.ToLower();

        var publicEndpoints = new[]
        {
            "/api/auth/login",
            "/api/auth/register",
            "/health",
            "/swagger"
        };

        return !publicEndpoints.Any(endpoint => path?.StartsWith(endpoint) == true);
    }
}