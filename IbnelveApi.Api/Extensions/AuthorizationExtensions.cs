using System;
using System.Linq;
using IbnelveApi.Application.Security;
using IbnelveApi.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace IbnelveApi.Api.Extensions
{
    /// <summary>
    /// Extensão para registrar políticas de autorização baseadas em Permission.
    /// </summary>
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddProjectAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddAuthorization(options =>
            {
                foreach (var perm in Enum.GetValues(typeof(Permission)).Cast<Permission>())
                {
                    options.AddPolicy(perm.ToString(), policy =>
                        policy.Requirements.Add(new PermissionRequirement(new[] { perm })));
                }
            });
            return services;
        }
    }
}