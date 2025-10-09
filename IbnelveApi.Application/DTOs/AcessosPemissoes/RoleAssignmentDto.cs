using System.Collections.Generic;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.DTOs.AcessosPemissoes
{
    /// <summary>
    /// DTO para atribuição de papéis e permissões a usuários via API.
    /// </summary>
    public class RoleAssignmentDto
    {
        public string UserId { get; set; }
        public string TenantId { get; set; }
        public Role Role { get; set; }
        public List<Permission> Permissions { get; set; } = new();
    }
}