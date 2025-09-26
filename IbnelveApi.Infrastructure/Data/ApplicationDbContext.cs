using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Entities.General;
using IbnelveApi.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IbnelveApi.Infrastructure.Data;

/// <summary>
/// ApplicationDbContext
/// LIMPO: Removido código comentado e simplificado filtros globais
/// </summary>
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Membro> Membros { get; set; }
    public DbSet<CategoriaUtensilio> Categoria { get; set; }
    public DbSet<Cidade> Cidades { get; set; }
    public DbSet<Utensilio> Utensilios { get; set; }
    public DbSet<LocalDeArmazenamento> LocaisDeArmazenamento { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configurações
        modelBuilder.ApplyConfiguration(new MembroConfiguration());
        modelBuilder.ApplyConfiguration(new CidadeConfiguration());
        modelBuilder.ApplyConfiguration(new UtensilioConfiguration());
        modelBuilder.ApplyConfiguration(new CategoriaUtensilioConfiguration());
        modelBuilder.ApplyConfiguration(new LocalDeArmazenamentoConfiguration());

        // Configurar filtros globais
        ConfigureGlobalFilters(modelBuilder);

        // Configurar Identity tables
        ConfigureIdentityTables(modelBuilder);

    }

    private void ConfigureGlobalFilters(ModelBuilder modelBuilder)
    {

        // Filtro para TenantEntity (como Membro) - apenas TenantId
        modelBuilder.Entity<Membro>().HasQueryFilter(e =>
            !e.IsDeleted &&
            GetCurrentTenantId() != null &&
            e.TenantId == GetCurrentTenantId());
    }

    private string? GetCurrentTenantId()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = httpContext.User.FindFirst("TenantId") ??
                             httpContext.User.FindFirst("Tenant_id") ??
                             httpContext.User.FindFirst(ClaimTypes.GroupSid);

            return tenantClaim?.Value;
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
        // Atualizar timestamps e TenantId automaticamente
        foreach (var entry in ChangeTracker.Entries<TenantEntity>())
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

        // Atualizar timestamps e UserId para UserOwnedEntity
        foreach (var entry in ChangeTracker.Entries<UserOwnedEntity>())
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

                    // Definir UserId automaticamente se não estiver definido
                    if (string.IsNullOrEmpty(entry.Entity.UserId))
                    {
                        var currentUserId = GetCurrentUserId();
                        if (!string.IsNullOrEmpty(currentUserId))
                        {
                            entry.Entity.UserId = currentUserId;
                        }
                    }
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    // Garantir que TenantId e UserId não sejam alterados em updates
                    entry.Property(e => e.TenantId).IsModified = false;
                    entry.Property(e => e.UserId).IsModified = false;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

