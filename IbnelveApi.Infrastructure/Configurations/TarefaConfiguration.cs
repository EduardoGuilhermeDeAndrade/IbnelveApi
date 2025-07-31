using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Infrastructure.Configurations;

public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("Tarefas");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(StatusTarefa.Pendente);

        builder.Property(t => t.Prioridade)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(PrioridadeTarefa.Media);

        builder.Property(t => t.DataVencimento);

        builder.Property(t => t.DataConclusao);

        builder.Property(t => t.Categoria)
            .HasMaxLength(100);

        builder.Property(t => t.TenantId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(t => t.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt);

        // Índices
        builder.HasIndex(t => new { t.TenantId, t.Status });
        builder.HasIndex(t => new { t.TenantId, t.Prioridade });
        builder.HasIndex(t => new { t.TenantId, t.Categoria });
        builder.HasIndex(t => new { t.TenantId, t.DataVencimento });
        builder.HasIndex(t => t.TenantId);
        builder.HasIndex(t => t.IsDeleted);
        builder.HasIndex(t => t.CreatedAt);
    }
}

