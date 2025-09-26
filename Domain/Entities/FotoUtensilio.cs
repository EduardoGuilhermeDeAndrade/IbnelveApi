public class FotoUtensilio : TenantEntity
{
    public Guid UtensilioId { get; set; }
    public string ArquivoPath { get; set; } = null!;
    public string? Descricao { get; set; }
    public bool IsPrincipal { get; set; }
    public Utensilio Utensilio { get; set; } = null!;
}