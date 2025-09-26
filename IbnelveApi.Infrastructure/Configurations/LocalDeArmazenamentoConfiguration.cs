using IbnelveApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IbnelveApi.Infrastructure.Configurations;

public class LocalDeArmazenamentoConfiguration : IEntityTypeConfiguration<LocalDeArmazenamento>
{
    public void Configure(EntityTypeBuilder<LocalDeArmazenamento> builder)
    {
        builder.ToTable("LocaisDeArmazenamento");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Descricao)
            .HasMaxLength(500);

        builder.Property(x => x.ContatoResponsavel)
            .HasMaxLength(200);

        builder.Property(x => x.TenantId)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasMany(x => x.Utensilios)
            .WithOne(x => x.LocalDeArmazenamento)
            .HasForeignKey(x => x.LocalDeArmazenamentoId)
            .IsRequired(false);
    }
}