using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Infrastructure.Data
{
    /// <summary>
    /// Seeder para popular usuários de teste com roles e permissões.
    /// Apenas para ambiente de desenvolvimento/teste. NÃO usar em produção.
    /// </summary>
    public static class UserPermissionsSeeder
    {
        public static async Task SeedUsersAndPermissionsAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var tenants = new[] { "tenant1", "tenant2" };
            var usersToSeed = new List<(string Email, string Role, string TenantId)>
            {
                ("admin1@ibnelveapi.com", "Admin", "tenant1"),
                ("secretario1@ibnelveapi.com", "Secretario", "tenant1"),
                ("user1@ibnelveapi.com", "User", "tenant1"),
                ("admin2@ibnelveapi.com", "Admin", "tenant2"),
                ("secretario2@ibnelveapi.com", "Secretario", "tenant2"),
                ("user2@ibnelveapi.com", "User", "tenant2")
            };

            var allPermissions = System.Enum.GetValues(typeof(Permission)).Cast<Permission>().Select(p => p.ToString()).ToList();
            var secretarioPermissions = new[] { Permission.Pessoa_Read, Permission.Pessoa_Create, Permission.Pessoa_Update, Permission.Tarefa_Read }.Select(p => p.ToString()).ToList();
            var userPermissions = new[] { Permission.Pessoa_Read, Permission.Tarefa_Read }.Select(p => p.ToString()).ToList();

            foreach (var (email, role, tenantId) in usersToSeed)
            {
                // Criar role se não existir
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

                // Verificar se usuário existe
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new IdentityUser { UserName = email, Email = email };
                    await userManager.CreateAsync(user, "Admin123!");
                }

                // Adicionar role ao usuário se não tiver
                if (!await userManager.IsInRoleAsync(user, role))
                    await userManager.AddToRoleAsync(user, role);

                // Claims de tenant
                var claims = await userManager.GetClaimsAsync(user);
                if (!claims.Any(c => c.Type == "TenantId" && c.Value == tenantId))
                    await userManager.AddClaimAsync(user, new Claim("TenantId", tenantId));

                // Claim de role
                if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role))
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));

                // Claims de permissões
                var permissions = role == "Admin" ? allPermissions : role == "Secretario" ? secretarioPermissions : userPermissions;
                foreach (var perm in permissions)
                {
                    if (!claims.Any(c => c.Type == "permission" && c.Value == perm))
                        await userManager.AddClaimAsync(user, new Claim("permission", perm));
                }
            }
        }
    }
}
