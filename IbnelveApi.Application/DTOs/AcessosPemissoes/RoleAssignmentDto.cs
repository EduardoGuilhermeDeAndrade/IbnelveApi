using System.Collections.Generic;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.DTOs.AcessosPemissoes
{
    /// <summary>
    /// DTO para atribui��o de pap�is e permiss�es a usu�rios via API.
    /// </summary>
    public class RoleAssignmentDto
    {
        public string UserId { get; set; }
        public string TenantId { get; set; }
        public Role Role { get; set; }
        public List<Permission> Permissions { get; set; } = new();
    }
}