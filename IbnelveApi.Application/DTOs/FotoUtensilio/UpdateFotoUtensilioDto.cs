using IbnelveApi.Application.Interfaces;

namespace IbnelveApi.Application.DTOs.FotoUtensilio
{
    public class UpdateFotoUtensilioDto
    {
        public string ArquivoPath { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool IsPrincipal { get; set; }
    }
}