
using IbnelveApi.Api.Middlewares;

namespace IbnelveApi.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}