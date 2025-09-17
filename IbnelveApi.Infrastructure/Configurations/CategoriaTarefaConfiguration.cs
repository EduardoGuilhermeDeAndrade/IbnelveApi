using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.ValueObjects;

namespace IbnelveApi.Infrastructure.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<CategoriaTarefa>
{
    public void Configure(EntityTypeBuilder<CategoriaTarefa> builder)
    {

        builder.ToTable("CategoriaTarefas");

        builder.HasKey(p => p.Id);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Descricao)
            .HasMaxLength(500);

        builder.Property(e => e.Cor)
            .HasMaxLength(7);

        builder.Property(e => e.Ativa)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(e => new { e.Nome, e.TenantId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(e => new { e.TenantId, e.Ativa });

        builder.HasIndex(e => e.CreatedAt);

        builder.HasMany(e => e.Tarefas)
            .WithOne(e => e.Categoria)
            .HasForeignKey(e => e.CategoriaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}