using IbnelveApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IbnelveApi.Infrastructure.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Descricao).HasMaxLength(500);
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(64);
        builder.HasMany(x => x.Utensilios)
               .WithOne(x => x.Categoria)
               .HasForeignKey(x => x.CategoriaId)
               .IsRequired();
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}