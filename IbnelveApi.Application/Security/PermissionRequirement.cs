using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.Security
{
    /// <summary>
    /// Requisito de autoriza��o baseado em permiss�es.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public IEnumerable<Permission> RequiredPermissions { get; }

        public PermissionRequirement(IEnumerable<Permission> requiredPermissions)
        {
            RequiredPermissions = requiredPermissions;
        }
    }
}