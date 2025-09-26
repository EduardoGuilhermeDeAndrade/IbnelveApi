public class UpdateUtensilioDto
{
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public List<UpdateFotoUtensilioDto>? Fotos { get; set; }
}