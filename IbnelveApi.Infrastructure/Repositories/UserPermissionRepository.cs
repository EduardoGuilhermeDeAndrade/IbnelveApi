using IbnelveApi.Application.Interfaces;
using IbnelveApi.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IbnelveApi.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação de IUserPermissionRepository usando armazenamento em memória (exemplo).
    /// </summary>
    public class UserPermissionRepository : IUserPermissionRepository
    {
        // Exemplo: armazenamento em memória. Substitua por acesso ao banco conforme necessário.
        private readonly Dictionary<(string userId, string tenantId), HashSet<Permission>> _permissions = new();

        public Task<ICollection<Permission>> GetPermissionsByUserIdAsync(string userId, string tenantId)
        {
            var key = (userId, tenantId);
            if (_permissions.TryGetValue(key, out var perms))
                return Task.FromResult((ICollection<Permission>)perms.ToList());
            return Task.FromResult((ICollection<Permission>)new List<Permission>());
        }

        public Task AddPermissionToUserAsync(string userId, string tenantId, Permission permission)
        {
            var key = (userId, tenantId);
            if (!_permissions.ContainsKey(key))
                _permissions[key] = new HashSet<Permission>();
            _permissions[key].Add(permission);
            return Task.CompletedTask;
        }

        public Task RemovePermissionFromUserAsync(string userId, string tenantId, Permission permission)
        {
            var key = (userId, tenantId);
            if (_permissions.TryGetValue(key, out var perms))
                perms.Remove(permission);
            return Task.CompletedTask;
        }
    }
}