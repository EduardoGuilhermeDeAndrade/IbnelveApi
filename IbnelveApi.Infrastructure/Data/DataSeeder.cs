using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.ValueObjects;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        //Seed de usuários admin
        await SeedUsersAsync(userManager, context);

        // Seed de membros
        await SeedMembrosAsync(context);

        // Seed de tarefas
        await SeedTarefasAsync(context);
    }

    private static async Task SeedUsersAsync(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        if (!await context.Users.AnyAsync())
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
    }

    private static async Task SeedMembrosAsync(ApplicationDbContext context)
    {
        if (!await context.Membros.IgnoreQueryFilters().AnyAsync())
        {
            var membros = new List<Membro>
            {
                new Membro(
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
                new Membro(
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

            await context.Membros.AddRangeAsync(membros);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedTarefasAsync(ApplicationDbContext context)
    {
        if (!await context.Tarefas.IgnoreQueryFilters().AnyAsync())
        {
            var tarefas = new List<Tarefa>
            {
                // Tarefas para tenant1, alterar o id do usuario conforme necessário
                new Tarefa(
                    "Implementar autenticação JWT",
                    "Configurar sistema de autenticação usando JWT Bearer tokens para a API",
                    "3159add8-ac6b-4e76-8d8b-69a4864b19ff",
                    "tenant1",
                    PrioridadeTarefa.Alta,
                    DateTime.UtcNow.AddDays(7),
                    "Desenvolvimento"
                ),
                new Tarefa(
                    "Criar documentação da API",
                    "Documentar todos os endpoints da API usando Swagger/OpenAPI",
                     "3159add8-ac6b-4e76-8d8b-69a4864b19ff",
                    "tenant1",
                    PrioridadeTarefa.Media,
                    DateTime.UtcNow.AddDays(14),
                    "Documentação"
                ),
                new Tarefa(
                    "Configurar CI/CD",
                    "Implementar pipeline de integração e deploy contínuo",
                     "3159add8-ac6b-4e76-8d8b-69a4864b19ff",
                    "tenant1",
                    PrioridadeTarefa.Baixa,
                    DateTime.UtcNow.AddDays(21),
                    "DevOps"
                ),
                new Tarefa(
                    "Revisar código da API",
                    "Fazer code review dos endpoints implementados",
                     "3159add8-ac6b-4e76-8d8b-69a4864b19ff",
                    "tenant1",
                    PrioridadeTarefa.Media,
                    DateTime.UtcNow.AddDays(-2), // Vencida
                    "Qualidade"
                ),

                // Tarefas para tenant2
                new Tarefa(
                    "Implementar sistema de tarefas",
                    "Criar CRUD completo para gestão de tarefas com filtros avançados",
                     "d39740e2-a94d-482b-9f32-751058592e1c",
                    "tenant2",
                    PrioridadeTarefa.Critica,
                    DateTime.UtcNow.AddDays(3),
                    "Desenvolvimento"
                ),
                new Tarefa(
                    "Testes unitários",
                    "Implementar testes unitários para todos os serviços",
                     "d39740e2-a94d-482b-9f32-751058592e1c",
                    "tenant2",
                    PrioridadeTarefa.Alta,
                    DateTime.UtcNow.AddDays(10),
                    "Testes"
                ),
                new Tarefa(
                    "Otimizar consultas do banco",
                    "Analisar e otimizar queries do Entity Framework",
                     "d39740e2-a94d-482b-9f32-751058592e1c",
                    "tenant2",
                    PrioridadeTarefa.Media,
                    DateTime.UtcNow.AddDays(15),
                    "Performance"
                ),
                new Tarefa(
                    "Backup do banco de dados",
                    "Configurar rotina de backup automático",
                     "d39740e2-a94d-482b-9f32-751058592e1c",
                    "tenant2",
                    PrioridadeTarefa.Baixa,
                    DateTime.UtcNow.AddDays(30),
                    "Infraestrutura"
                )
            };

            // Marcar algumas tarefas como concluídas
            tarefas[1].MarcarComoConcluida(); // Documentação da API
            tarefas[5].MarcarComoConcluida(); // Testes unitários

            // Marcar algumas como em andamento
            tarefas[0].MarcarComoEmAndamento(); // Autenticação JWT
            tarefas[4].MarcarComoEmAndamento(); // Sistema de tarefas

            await context.Tarefas.AddRangeAsync(tarefas);
            await context.SaveChangesAsync();
        }
    }


}

