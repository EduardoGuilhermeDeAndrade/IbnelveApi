using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.ValueObjects;
using IbnelveApi.Infrastructure.Configurations;

namespace IbnelveApi.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Pessoa> Pessoas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configurações
        modelBuilder.ApplyConfiguration(new PessoaConfiguration());

        // Configurar filtros globais para soft delete e multi-tenancy
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Filtro global para soft delete
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);
                method.Invoke(null, new object[] { modelBuilder });
            }
        }

        // Configurar Identity tables para incluir TenantId
        ConfigureIdentityTables(modelBuilder);
    }

    private static void SetSoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
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
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Atualizar timestamps
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

