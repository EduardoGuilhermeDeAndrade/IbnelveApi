using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Enums;
using IbnelveApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IbnelveApi.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed de usuários com roles e permissões
        await UserPermissionsSeeder.SeedUsersAndPermissionsAsync(userManager, roleManager);

        //Seed de usuários admin
        await SeedUsersAsync(userManager, context);

        // Seed de Membros
        await SeedMembrosAsync(context);

        // Seed de categoriaTarefas
        await SeedCategoriaTarefas(context);

        // Seed de tarefas
        await SeedTarefasAsync(context);
        
        // Seed de Países e Estados
        await SeedPaisesEstadosAsync(context);
        
        // Seed de Categorias de Utensílios
        await SeedCategoriaUtensiliosAsync(context);
        await SeedUtensiliosAsync(context);
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
                        "SP",
                        "Brasil"
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
                        "RJ",
                        "Brasil"
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
                    PrioridadeTarefa.Alta,
                    DateTime.UtcNow.AddDays(7),
                    1,
                    "tenant1",
                    "3159add8-ac6b-4e76-8d8b-69a4864b19ff"
                ),
                new Tarefa(
                    "Criar documentação da API",
                    "Documentar todos os endpoints da API usando Swagger/OpenAPI",
                    PrioridadeTarefa.Media,
                    DateTime.UtcNow.AddDays(14),
                    1,
                    "tenant1",
                     "3159add8-ac6b-4e76-8d8b-69a4864b19ff"
                ),
                new Tarefa(
                    "Configurar CI/CD",
                    "Implementar pipeline de integração e deploy contínuo",
                    PrioridadeTarefa.Baixa,
                    DateTime.UtcNow.AddDays(21),
                    1,
                    "tenant1",
                    "3159add8-ac6b-4e76-8d8b-69a4864b19ff"
                ),
                new Tarefa(
                    "Revisar código da API",
                    "Fazer code review dos endpoints implementados",
                    PrioridadeTarefa.Media,
                    DateTime.UtcNow.AddDays(-2), // Vencida
                    1,
                    "3159add8-ac6b-4e76-8d8b-69a4864b19ff",
                    "tenant1"
                ),

                // Tarefas para tenant2
                new Tarefa(
                    "Implementar sistema de tarefas",
                    "Criar CRUD completo para gestão de tarefas com filtros avançados",
                    PrioridadeTarefa.Critica,
                    DateTime.UtcNow.AddDays(3),
                    1,
                    "tenant2",
                    "d39740e2-a94d-482b-9f32-751058592e1c"
                     ),
                new Tarefa(
                    "Testes unitários",
                    "Implementar testes unitários para todos os serviços",
                    PrioridadeTarefa.Alta,
                    DateTime.UtcNow.AddDays(10),
                    1,
                    "tenant2",
                    "d39740e2-a94d-482b-9f32-751058592e1c"
                ),
                new Tarefa(
                    "Otimizar consultas do banco",
                    "Analisar e otimizar queries do Entity Framework",
                    PrioridadeTarefa.Media,
                    DateTime.UtcNow.AddDays(15),
                    1,   
                    "tenant2",  
                    "d39740e2-a94d-482b-9f32-751058592e1c"
                ),
                new Tarefa(
                    "Backup do banco de dados",
                    "Configurar rotina de backup automático",
                    PrioridadeTarefa.Baixa,
                    DateTime.UtcNow.AddDays(30),
                    1,
                    "tenant2",
                    "d39740e2-a94d-482b-9f32-751058592e1c"                
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

    private static async Task SeedCategoriaTarefas(ApplicationDbContext context)
    {
        if (!await context.CategoriaTarefas.IgnoreQueryFilters().AnyAsync())
        {
            var categoriaTarefas = new List<CategoriaTarefa>
            {
                new CategoriaTarefa
                {
                   // Id = 1,
                    Nome = "Pessoal Estudos",
                    Descricao = "Tarefas relacionadas a estudos pessoais",
                    Cor = "#3B82F6", // Azul
                    Ativa = true,
                    TenantId = "tenant1",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CategoriaTarefa
                {
                    // Id = 2,
                    Nome = "Casa Manutenção",
                    Descricao = "Tarefas de manutenção da casa",
                    Cor = "#10B981", // Verde
                    Ativa = true,
                    TenantId = "tenant1",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CategoriaTarefa
                {
                    // Id = 3,
                    Nome = "Igreja Administração",
                    Descricao = "Tarefas administrativas da igreja",
                    Cor = "#8B5CF6", // Roxo
                    Ativa = true,
                    TenantId = "tenant1",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CategoriaTarefa
                {
                   //  Id = 4,
                    Nome = "Igreja Manutenção",
                    Descricao = "Tarefas de manutenção da igreja",
                    Cor = "#F59E0B", // Amarelo
                    Ativa = true,
                    TenantId = "tenant1",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CategoriaTarefa
                {
                   //  Id = 5,
                    Nome = "Veiculos",
                    Descricao = "Tarefas relacionadas a veículos",
                    Cor = "#EF4444", // Vermelho
                    Ativa = true,
                    TenantId = "tenant1",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },

                // Tenant 2
                new CategoriaTarefa
                {
                  //   Id = 6,
                    Nome = "Pessoal Estudos",
                    Descricao = "Tarefas relacionadas a estudos pessoais",
                    Cor = "#3B82F6", // Azul
                    Ativa = true,
                    TenantId = "tenant2",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CategoriaTarefa
                {
                   //  Id = 7,
                    Nome = "Casa Manutenção",
                    Descricao = "Tarefas de manutenção da casa",
                    Cor = "#10B981", // Verde
                    Ativa = true,
                    TenantId = "tenant2",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new CategoriaTarefa
                {
                   //  Id = 8,
                    Nome = "Veiculos",
                    Descricao = "Tarefas relacionadas a veículos",
                    Cor = "#EF4444", // Vermelho
                    Ativa = true,
                    TenantId = "tenant2",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            };

            await context.CategoriaTarefas.AddRangeAsync(categoriaTarefas);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedPaisesEstadosAsync(ApplicationDbContext context)
    {
        // Seed País Brasil
        var paisBrasil = await context.Set<Pais>().FirstOrDefaultAsync(p => p.Nome == "Brasil");
        if (paisBrasil == null)
        {
            paisBrasil = new Pais
            {
                Nome = "Brasil",
                CodigoISO2 = "BR",
                CodigoISO3 = "BRA",
                CodigoTelefone = "+55",
                Ativo = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            context.Set<Pais>().Add(paisBrasil);
            await context.SaveChangesAsync();
        }

        // Seed Estado Minas Gerais
        var estadoMG = await context.Set<Estado>().FirstOrDefaultAsync(e => e.Nome == "Minas Gerais" && e.Sigla == "MG");
        if (estadoMG == null)
        {
            estadoMG = new Estado
            {
                Nome = "Minas Gerais",
                Sigla = "MG",
                Ativo = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            // Relacionamento
            if (paisBrasil != null)
            {
                estadoMG.PaisId = paisBrasil.Id;
            }
            context.Set<Estado>().Add(estadoMG);
            await context.SaveChangesAsync();
        }

        // Seed Cidade Belo Horizonte (Minas Gerais)
        //var cidadeBH = await context.Set<Cidade>().FirstOrDefaultAsync(c => c.Nome == "Belo Horizonte" && c.UF == "MG");
        //if (cidadeBH == null && estadoMG != null)
        //{
        //    cidadeBH = new Cidade
        //    {
        //        Nome = "Belo Horizonte",
        //        UF = "MG",
        //        CEP = "30140071",
        //        Ativo = true,
        //        Capital = true,
        //        EstadoId = estadoMG.Id,
        //        CodigoIBGE = "3106200",
        //        CreatedAt = DateTime.UtcNow,
        //        IsDeleted = false
        //    };
        //    context.Set<Cidade>().Add(cidadeBH);
        //    await context.SaveChangesAsync();
        //}

        // Seed de todas as cidades de Minas Gerais
        var cidadesMG = new[]
        {
            new { Nome = "Belo Horizonte", CodigoIBGE = "3106200" },
            new { Nome = "Uberlândia", CodigoIBGE = "3170206" },
            new { Nome = "Contagem", CodigoIBGE = "3118601" },
            new { Nome = "Juiz de Fora", CodigoIBGE = "3136702" },
            new { Nome = "Betim", CodigoIBGE = "3106705" },
            new { Nome = "Montes Claros", CodigoIBGE = "3143302" },
            new { Nome = "Ribeirão das Neves", CodigoIBGE = "3154606" },
            new { Nome = "Uberaba", CodigoIBGE = "3170107" },
            new { Nome = "Governador Valadares", CodigoIBGE = "3127701" },
            new { Nome = "Ipatinga", CodigoIBGE = "3131307" },
            new { Nome = "Divinópolis", CodigoIBGE = "3122306" },
            new { Nome = "Sete Lagoas", CodigoIBGE = "3167202" },
            new { Nome = "Santa Luzia", CodigoIBGE = "3157807" },
            new { Nome = "Ibirité", CodigoIBGE = "3129806" },
            new { Nome = "Poços de Caldas", CodigoIBGE = "3151800" },
            new { Nome = "Patos de Minas", CodigoIBGE = "3148004" },
            new { Nome = "Teófilo Otoni", CodigoIBGE = "3168606" },
            new { Nome = "Sabará", CodigoIBGE = "3156700" },
            new { Nome = "Barbacena", CodigoIBGE = "3105608" },
            new { Nome = "Varginha", CodigoIBGE = "3170701" }
            // ...adicione mais cidades conforme necessário...
        };
        if (estadoMG != null)
        {
            foreach (var cidade in cidadesMG)
            {
                var exists = await context.Set<Cidade>().AnyAsync(c => c.Nome == cidade.Nome && c.UF == "MG");
                if (!exists)
                {
                    var novaCidade = new Cidade
                    {
                        Nome = cidade.Nome,
                        UF = "MG",
                        CEP = string.Empty,
                        Ativo = true,
                        Capital = cidade.Nome == "Belo Horizonte",
                        EstadoId = estadoMG.Id,
                        CodigoIBGE = cidade.CodigoIBGE,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    context.Set<Cidade>().Add(novaCidade);
                }
            }
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedCategoriaUtensiliosAsync(ApplicationDbContext context)
    {
        if (!await context.Categoria.AnyAsync())
        {
            var categorias = new List<CategoriaUtensilio>
            {
                new CategoriaUtensilio("Cozinha", "Utensílios de cozinha", "tenant1", "system"),
                new CategoriaUtensilio("Escritório", "Utensílios de escritório", "tenant1", "system"),
                new CategoriaUtensilio("Cozinha", "Utensílios de cozinha", "tenant2", "system"),
                new CategoriaUtensilio("Escritório", "Utensílios de escritório", "tenant2", "system")
            };

            await context.Categoria.AddRangeAsync(categorias);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedUtensiliosAsync(ApplicationDbContext context)
    {
        if (!await context.Utensilios.AnyAsync())
        {
            // Buscar categorias por tenant
            var categoriasTenant1 = await context.Categoria
                .Where(c => c.TenantId == "tenant1")
                .ToListAsync();
            var categoriasTenant2 = await context.Categoria
                .Where(c => c.TenantId == "tenant2")
                .ToListAsync();

            // Garantir que temos as duas categorias por tenant
            var cozinha1 = categoriasTenant1.FirstOrDefault(c => c.Nome == "Cozinha");
            var escritorio1 = categoriasTenant1.FirstOrDefault(c => c.Nome == "Escritório");
            var cozinha2 = categoriasTenant2.FirstOrDefault(c => c.Nome == "Cozinha");
            var escritorio2 = categoriasTenant2.FirstOrDefault(c => c.Nome == "Escritório");

            var utensilios = new List<Utensilio>
            {
                // Tenant 1
                new Utensilio("Geladeira", "Geladeira duplex", "Nova", 3500, DateTime.UtcNow.AddYears(-1), "SN123", "Electrolux", StatusItem.Ativo, cozinha1?.Id ?? 0, "tenant1"),
                new Utensilio("Fogão", "Fogão 4 bocas", "Com acendimento automático", 1200, DateTime.UtcNow.AddYears(-2), "SN456", "Brastemp", StatusItem.Ativo, cozinha1?.Id ?? 0, "tenant1"),
                new Utensilio("Notebook", "Notebook Dell", "Inspiron 15", 4500, DateTime.UtcNow.AddMonths(-6), "SN789", "Dell", StatusItem.Ativo, escritorio1?.Id ?? 0, "tenant1"),
                new Utensilio("Projetor", "Projetor Epson", "HD", 2500, DateTime.UtcNow.AddMonths(-3), "SN321", "Epson", StatusItem.Ativo, escritorio1?.Id ?? 0, "tenant1"),
                new Utensilio("Cafeteira", "Cafeteira elétrica", "Preta", 300, DateTime.UtcNow.AddMonths(-1), "SN654", "Philco", StatusItem.Ativo, cozinha1?.Id ?? 0, "tenant1"),

                // Tenant 2
                new Utensilio("Geladeira", "Geladeira Frost Free", "Branca", 3200, DateTime.UtcNow.AddYears(-2), "SN987", "Consul", StatusItem.Ativo, cozinha2?.Id ?? 0, "tenant2"),
                new Utensilio("Fogão", "Fogão 5 bocas", "Com timer", 1500, DateTime.UtcNow.AddYears(-1), "SN654", "Electrolux", StatusItem.Ativo, cozinha2?.Id ?? 0, "tenant2"),
                new Utensilio("Notebook", "Notebook Lenovo", "ThinkPad", 5000, DateTime.UtcNow.AddMonths(-8), "SN852", "Lenovo", StatusItem.Ativo, escritorio2?.Id ?? 0, "tenant2"),
                new Utensilio("Projetor", "Projetor LG", "Full HD", 2700, DateTime.UtcNow.AddMonths(-2), "SN963", "LG", StatusItem.Ativo, escritorio2?.Id ?? 0, "tenant2"),
                new Utensilio("Cafeteira", "Cafeteira Nespresso", "Vermelha", 600, DateTime.UtcNow.AddMonths(-4), "SN741", "Nespresso", StatusItem.Ativo, cozinha2?.Id ?? 0, "tenant2")
            };

            await context.Utensilios.AddRangeAsync(utensilios);
            await context.SaveChangesAsync();
        }
    }
}

