using IbnelveApi.Domain.Entities.General;
using System.Collections.Generic;

namespace IbnelveApi.Domain.Entities
{
    public class Pais : GlobalEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string CodigoISO2 { get; set; } = string.Empty;
        public string CodigoISO3 { get; set; } = string.Empty;
        public string? CodigoTelefone { get; set; }
        public bool Ativo { get; set; } = true;
        public virtual ICollection<Estado> Estados { get; set; } = new List<Estado>();
    }
}
