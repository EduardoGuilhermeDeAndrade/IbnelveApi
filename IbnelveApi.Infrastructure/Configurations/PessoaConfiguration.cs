using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.ValueObjects;

namespace IbnelveApi.Infrastructure.Configurations;

public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.ToTable("Pessoas");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.CPF)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(p => p.Telefone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.TenantId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);

        // Configuração do Value Object Endereco
        builder.OwnsOne(p => p.Endereco, endereco =>
        {
            endereco.Property(e => e.Rua)
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnName("EnderecoRua");

            endereco.Property(e => e.CEP)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("EnderecoCEP");

            endereco.Property(e => e.Bairro)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("EnderecoBairro");

            endereco.Property(e => e.Cidade)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("EnderecoCidade");

            endereco.Property(e => e.UF)
                .IsRequired()
                .HasMaxLength(2)
                .HasColumnName("EnderecoUF");
        });

        // Índices
        builder.HasIndex(p => new { p.TenantId, p.CPF })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(p => p.TenantId);
        builder.HasIndex(p => p.IsDeleted);
    }
}

