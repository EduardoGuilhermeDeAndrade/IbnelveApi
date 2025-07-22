using IbnelveApi.Domain.Interfaces;

namespace IbnelveApi.Api.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        // Tentar obter o TenantId do header X-Tenant-Id
        var tenantId = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        
        // Se não encontrar no header, usar um valor padrão
        if (string.IsNullOrEmpty(tenantId))
        {
            tenantId = "tenant1"; // Valor padrão para demonstração
        }

        // Definir o TenantId no contexto
        tenantContext.SetTenantId(tenantId);

        await _next(context);
    }
}

public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}

