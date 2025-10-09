using System.Threading.Tasks;
using IbnelveApi.Domain.Enums;
using IbnelveApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IbnelveApi.Application.DTOs.AcessosPemissoes;

namespace IbnelveApi.Api.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de roles e permissões de usuários (somente Admin).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Configuracao_Edit")]
    public class PermissionsController : ControllerBase
    {
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;

        public PermissionsController(IUserPermissionRepository userPermissionRepository, IAuthorizationService authorizationService, ICurrentUserService currentUserService)
        {
            _userPermissionRepository = userPermissionRepository;
            _authorizationService = authorizationService;
            _currentUserService = currentUserService;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserPermissions(string id)
        {
            var perms = await _userPermissionRepository.GetPermissionsByUserIdAsync(id, _currentUserService.GetTenantId());
            return Ok(new ApiResponse<ICollection<Permission>>(true, perms));
        }

        [HttpPost("user/{id}/assign")]
        public async Task<IActionResult> AssignPermissions(string id, [FromBody] RoleAssignmentDto dto)
        {
            foreach (var perm in dto.Permissions)
                await _userPermissionRepository.AddPermissionToUserAsync(id, dto.TenantId, perm);
            return Ok(new ApiResponse<string>(true, "Permissões atribuídas"));
        }

        [HttpPost("user/{id}/revoke")]
        public async Task<IActionResult> RevokePermissions(string id, [FromBody] RoleAssignmentDto dto)
        {
            foreach (var perm in dto.Permissions)
                await _userPermissionRepository.RemovePermissionFromUserAsync(id, dto.TenantId, perm);
            return Ok(new ApiResponse<string>(true, "Permissões revogadas"));
        }
    }
}