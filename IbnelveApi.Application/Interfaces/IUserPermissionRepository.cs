using System.Collections.Generic;
using System.Threading.Tasks;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Application.Interfaces
{
    /// <summary>
    /// Interface para acesso e manipula��o de permiss�es de usu�rio multi-tenant.
    /// </summary>
    public interface IUserPermissionRepository
    {
        Task<ICollection<Permission>> GetPermissionsByUserIdAsync(string userId, string tenantId);
        Task AddPermissionToUserAsync(string userId, string tenantId, Permission permission);
        Task RemovePermissionFromUserAsync(string userId, string tenantId, Permission permission);
    }
}