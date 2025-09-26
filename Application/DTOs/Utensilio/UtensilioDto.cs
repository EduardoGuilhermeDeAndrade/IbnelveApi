public class UtensilioDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public List<FotoUtensilioDto> Fotos { get; set; } = new();
}