using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Extensions;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;

namespace IbnelveApi.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    //private readonly ITenantContext _tenantContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,  IHttpContextAccessor httpContextAccessor/*ITenantContext tenantContext*/)
        : base(options)
    {
        //_tenantContext = tenantContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configurações
        modelBuilder.ApplyConfiguration(new PessoaConfiguration());
        modelBuilder.ApplyConfiguration(new TarefaConfiguration());

        // Configurar filtros globais para todas as entidades que herdam de BaseEntity
        ConfigureGlobalFilters(modelBuilder);

        // Configurar Identity tables para incluir TenantId
        ConfigureIdentityTables(modelBuilder);
    }

    private void ConfigureGlobalFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.MakeGenericMethod(entityType.ClrType);

                method?.Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    //private void SetGlobalQueryFilter<T>(ModelBuilder modelBuilder)
    //    where T : BaseEntity
    //{
    //    modelBuilder.Entity<T>().HasQueryFilter(e =>
    //        !e.IsDeleted &&
    //        GetCurrentTenantId() != null &&
    //        e.TenantId == GetCurrentTenantId()) 
    //        ;
    //}
    private void SetGlobalQueryFilter<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
        var tenantId = GetCurrentTenantId();
        var userId = GetCurrentUserId();

        // Tenant obrigatório sempre
        Expression<Func<T, bool>> filter = e =>
            !e.IsDeleted &&
            tenantId != null &&
            e.TenantId == tenantId;

        // Se a entidade também for "user-scoped", aplicar filtro adicional
        if (typeof(IUserScopedEntity).IsAssignableFrom(typeof(T)))
        {
            Expression<Func<T, bool>> userFilter = e =>
                ((IUserScopedEntity)e).UserId == userId;

            // Combina com AND lógico
            filter = filter.And(userFilter);
        }

        modelBuilder.Entity<T>().HasQueryFilter(filter);
    }


    private string? GetCurrentTenantId()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = httpContext.User.FindFirst("TenantId")
                             ?? httpContext.User.FindFirst("Tenant_id")
                             ?? httpContext.User.FindFirst(ClaimTypes.GroupSid);

            return tenantClaim?.Value; // só acessa Value se não for null
        }

        return null;
    }


    private string? GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier) ??
                              httpContext.User.FindFirst("sub");

            return userIdClaim?.Value;
        }

        return null;
    }


    private void ConfigureIdentityTables(ModelBuilder modelBuilder)
    {
        //Todo: Verificar se é necessário criar uma classe customizada para IdentityUser e IdentityRole para incluir TenantId diretamente
        // Adicionar TenantId às tabelas do Identity
        modelBuilder.Entity<IdentityUser>().Property<string>("TenantId").HasMaxLength(450);
        modelBuilder.Entity<IdentityUser>().HasIndex("TenantId");

        // Configurar nomes das tabelas
        modelBuilder.Entity<IdentityUser>().ToTable("Users");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Atualizar timestamps automaticamente
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    // Definir TenantId automaticamente se não estiver definido
                    if (string.IsNullOrEmpty(entry.Entity.TenantId))
                    {
                        var currentTenantId = GetCurrentTenantId();
                        if (!string.IsNullOrEmpty(currentTenantId))
                        {
                            entry.Entity.TenantId = currentTenantId;
                        }
                    }
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    // Garantir que o TenantId não seja alterado em updates
                    entry.Property(e => e.TenantId).IsModified = false;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    
}



