namespace IbnelveApi.Application.DTOs.FotoUtensilio
{
    public class FotoUtensilioDto
    {
        public int Id { get; set; }
        public string ArquivoPath { get; set; } = null!;
        public string? Descricao { get; set; }
        public bool IsPrincipal { get; set; }
    }
}