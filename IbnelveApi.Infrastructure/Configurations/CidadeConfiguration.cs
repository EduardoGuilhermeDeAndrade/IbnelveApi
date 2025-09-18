using IbnelveApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace IbnelveApi.Infrastructure.Configurations;

public class CidadeConfiguration : IEntityTypeConfiguration<Cidade>
{
    public void Configure(EntityTypeBuilder<Cidade> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).IsRequired().HasMaxLength(200);
        builder.Property(c => c.UF).IsRequired().HasMaxLength(2);
        builder.Property(c => c.CEP).IsRequired().HasMaxLength(8);
        builder.Property(c => c.Ativo).IsRequired();
        builder.Property(c => c.Capital).IsRequired();
        builder.Property(c => c.CodigoIBGE).HasMaxLength(10);
        builder.HasOne(c => c.Estado)
            .WithMany(e => e.Cidades)
            .HasForeignKey(c => c.EstadoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
