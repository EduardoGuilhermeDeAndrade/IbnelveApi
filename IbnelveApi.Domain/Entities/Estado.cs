using IbnelveApi.Domain.Entities.General;
using System.Collections.Generic;

namespace IbnelveApi.Domain.Entities
{
    public class Estado : GlobalEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty; // Ex: SP, RJ
        public bool Ativo { get; set; } = true;
        public int PaisId { get; set; } // Relacionamento com País
        public virtual Pais? Pais { get; set; }
        public virtual ICollection<Cidade> Cidades { get; set; } = new List<Cidade>();
    }
}
