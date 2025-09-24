using IbnelveApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IbnelveApi.Infrastructure.Configurations;

public class UtensilioConfiguration : IEntityTypeConfiguration<Utensilio>
{
    public void Configure(EntityTypeBuilder<Utensilio> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Descricao).HasMaxLength(500);
        builder.Property(x => x.Observacoes).HasMaxLength(500);
        builder.Property(x => x.ValorReferencia).HasColumnType("decimal(18,2)");
        builder.Property(x => x.NumeroSerie).HasMaxLength(100);
        builder.Property(x => x.NomeFornecedor).HasMaxLength(100);
        builder.Property(x => x.Situacao).IsRequired();
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(64);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}