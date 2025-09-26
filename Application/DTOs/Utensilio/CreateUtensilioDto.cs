public class CreateUtensilioDto
{
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public List<CreateFotoUtensilioDto>? Fotos { get; set; }
}