using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FotoUtensilioConfiguration : IEntityTypeConfiguration<FotoUtensilio>
{
    public void Configure(EntityTypeBuilder<FotoUtensilio> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ArquivoPath).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Descricao).HasMaxLength(255);
        builder.Property(x => x.IsPrincipal).IsRequired();

        builder.HasOne(x => x.Utensilio)
            .WithMany(x => x.Fotos)
            .HasForeignKey(x => x.UtensilioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt);
    }
}