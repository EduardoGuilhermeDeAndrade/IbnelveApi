using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Aplicar migrations
        await context.Database.MigrateAsync();

        // Seed de usuário admin
        //await SeedAdminUserAsync(userManager);

        // Seed de produtos
        //await SeedProdutosAsync(context);
    }

    private static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager)
    {
        const string adminEmail = "admin@ibnelveapi.com";
        const string adminPassword = "Admin@123";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, adminPassword);
        }
    }

    private static async Task SeedProdutosAsync(ApplicationDbContext context)
    {
        if (!await context.Produtos.AnyAsync())
        {
            var produtos = new List<Produto>
            {
                new()
                {
                    Nome = "Notebook Dell Inspiron",
                    Preco = 2500.00m,
                    TenantId = "tenant1",
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Nome = "Mouse Logitech MX Master",
                    Preco = 350.00m,
                    TenantId = "tenant1",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Produtos.AddRangeAsync(produtos);
            await context.SaveChangesAsync();
        }
    }
}

