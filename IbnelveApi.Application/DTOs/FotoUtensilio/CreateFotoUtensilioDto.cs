namespace IbnelveApi.Application.DTOs.FotoUtensilio
{
    public class CreateFotoUtensilioDto
    {
        public string ArquivoPath { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool IsPrincipal { get; set; }
    }
}