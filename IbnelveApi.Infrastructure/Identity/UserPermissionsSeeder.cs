using IbnelveApi.Application.Interfaces;
using IbnelveApi.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IbnelveApi.Infrastructure.Identity
{
    /// <summary>
    /// Seeder para usuários de exemplo e associação de roles/permissões (apenas dev).
    /// </summary>
    public static class UserPermissionsSeeder
    {
        public static async Task SeedAsync(IUserPermissionRepository repo)
        {
            // Admin
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Pessoa_Read);
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Pessoa_Create);
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Pessoa_Update);
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Pessoa_Delete);
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Tarefa_Read);
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Tarefa_Create);
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Tarefa_Update);
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Tarefa_Delete);
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Relatorio_View);
            await repo.AddPermissionToUserAsync("admin@tenant.com", "tenant1", Permission.Configuracao_Edit);

            // Secretario
            await repo.AddPermissionToUserAsync("secretario@tenant.com", "tenant1", Permission.Pessoa_Read);
            await repo.AddPermissionToUserAsync("secretario@tenant.com", "tenant1", Permission.Pessoa_Create);
            await repo.AddPermissionToUserAsync("secretario@tenant.com", "tenant1", Permission.Pessoa_Update);
            await repo.AddPermissionToUserAsync("secretario@tenant.com", "tenant1", Permission.Tarefa_Read);
            await repo.AddPermissionToUserAsync("secretario@tenant.com", "tenant1", Permission.Tarefa_Create);
            await repo.AddPermissionToUserAsync("secretario@tenant.com", "tenant1", Permission.Relatorio_View);

            // User
            await repo.AddPermissionToUserAsync("user@tenant.com", "tenant1", Permission.Pessoa_Read);
            await repo.AddPermissionToUserAsync("user@tenant.com", "tenant1", Permission.Tarefa_Read);
        }
    }
}