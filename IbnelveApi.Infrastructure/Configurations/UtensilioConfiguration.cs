using IbnelveApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IbnelveApi.Infrastructure.Configurations;

public class UtensilioConfiguration : IEntityTypeConfiguration<Utensilio>
{
    public void Configure(EntityTypeBuilder<Utensilio> builder)
    {
        builder.ToTable("Utensilios");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Descricao)
            .HasMaxLength(500);

        builder.Property(x => x.TenantId)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasOne(x => x.Categoria)
            .WithMany(x => x.Utensilios)
            .HasForeignKey(x => x.CategoriaId)
            .IsRequired(false);

        builder.HasOne(x => x.LocalDeArmazenamento)
            .WithMany(x => x.Utensilios)
            .HasForeignKey(x => x.LocalDeArmazenamentoId)
            .IsRequired(false);

        builder.HasMany(x => x.Fotos)
            .WithOne(x => x.Utensilio)
            .HasForeignKey(x => x.UtensilioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}