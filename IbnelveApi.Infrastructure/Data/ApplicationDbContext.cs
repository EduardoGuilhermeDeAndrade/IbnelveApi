using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Common;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IbnelveApi.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    private readonly ITenantContext _tenantContext;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configurar filtros globais para todas as entidades que herdam de BaseEntity
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Filtro global para exclusão lógica
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SetGlobalQueryFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);
                method?.Invoke(null, new object[] { builder, _tenantContext });
            }
        }

        // Configurações específicas das entidades
        builder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Preco).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TenantId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => new { e.TenantId, e.IsDeleted });
        });
    }

    private static void SetGlobalQueryFilter<T>(ModelBuilder builder, ITenantContext tenantContext) where T : BaseEntity
    {
        builder.Entity<T>().HasQueryFilter(e => 
            !e.IsDeleted && 
            (string.IsNullOrEmpty(tenantContext.TenantId) || e.TenantId == tenantContext.TenantId));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    if (string.IsNullOrEmpty(entry.Entity.TenantId))
                    {
                        entry.Entity.TenantId = _tenantContext.TenantId;
                    }
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

