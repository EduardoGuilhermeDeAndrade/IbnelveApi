using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.ValueObjects;

namespace IbnelveApi.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        // Seed de usuários admin
        await SeedUsersAsync(userManager);

        // Seed de pessoas
        await SeedPessoasAsync(context);
    }

    private static async Task SeedUsersAsync(UserManager<IdentityUser> userManager)
    {
        // Usuário admin para tenant1
        var adminUser1 = await userManager.FindByEmailAsync("admin1@ibnelveapi.com");
        if (adminUser1 == null)
        {
            adminUser1 = new IdentityUser
            {
                UserName = "admin1@ibnelveapi.com",
                Email = "admin1@ibnelveapi.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser1, "Admin123!");
            if (result.Succeeded)
            {
                // Adicionar TenantId via claim ou propriedade customizada
                await userManager.AddClaimAsync(adminUser1, new System.Security.Claims.Claim("TenantId", "tenant1"));
            }
        }

        // Usuário admin para tenant2
        var adminUser2 = await userManager.FindByEmailAsync("admin2@ibnelveapi.com");
        if (adminUser2 == null)
        {
            adminUser2 = new IdentityUser
            {
                UserName = "admin2@ibnelveapi.com",
                Email = "admin2@ibnelveapi.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser2, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(adminUser2, new System.Security.Claims.Claim("TenantId", "tenant2"));
            }
        }
    }

    private static async Task SeedPessoasAsync(ApplicationDbContext context)
    {
        if (!await context.Pessoas.AnyAsync())
        {
            var pessoas = new List<Pessoa>
            {
                new Pessoa(
                    "João Silva Santos",
                    "12345678901",
                    "(11) 99999-1111",
                    new Endereco(
                        "Rua das Flores, 123",
                        "01234567",
                        "Centro",
                        "São Paulo",
                        "SP"
                    ),
                    "tenant1"
                ),
                new Pessoa(
                    "Maria Oliveira Costa",
                    "98765432109",
                    "(21) 88888-2222",
                    new Endereco(
                        "Avenida Atlântica, 456",
                        "22070900",
                        "Copacabana",
                        "Rio de Janeiro",
                        "RJ"
                    ),
                    "tenant2"
                )
            };

            await context.Pessoas.AddRangeAsync(pessoas);
            await context.SaveChangesAsync();
        }
    }
}

