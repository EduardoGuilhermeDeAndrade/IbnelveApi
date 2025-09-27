using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IbnelveApi.Domain.Enums;
using IbnelveApi.Application.Interfaces;

namespace IbnelveApi.Application.Security
{
    /// <summary>
    /// Handler de autorização para permissões e roles, respeitando multi-tenancy.
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserPermissionRepository _userPermissionRepository;

        public PermissionHandler(ICurrentUserService currentUserService, IUserPermissionRepository userPermissionRepository)
        {
            _currentUserService = currentUserService;
            _userPermissionRepository = userPermissionRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = _currentUserService.GetUserId();
            var tenantId = _currentUserService.GetTenantId();
            var userRole = _currentUserService.GetUserRole();

            // Admin pode tudo
            if (userRole == Role.Admin)
            {
                context.Succeed(requirement);
                return;
            }

            // Verifica claims de permissões
            var permissionClaims = context.User.FindAll("permission").Select(c => c.Value).ToList();
            if (permissionClaims.Any())
            {
                var requiredNames = requirement.RequiredPermissions.Select(p => p.ToString());
                if (requiredNames.Any(rn => permissionClaims.Contains(rn)))
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            // Fallback: consulta repositório
            var userPermissions = await _userPermissionRepository.GetPermissionsByUserIdAsync(userId, tenantId);
            if (requirement.RequiredPermissions.Any(rp => userPermissions.Contains(rp)))
            {
                context.Succeed(requirement);
            }
        }
    }
}