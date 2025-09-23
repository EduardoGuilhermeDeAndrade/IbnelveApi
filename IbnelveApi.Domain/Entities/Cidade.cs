using System.Collections.Generic;
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Entities
{
    /// <summary>
    /// Entidade global - Cidade
    /// Compartilhada entre TODOS os tenants
    /// </summary>
    public class Cidade : GlobalEntity
    {
        public string Nome { get; set; } = string.Empty;
        
        public string UF { get; set; } = string.Empty; // Ex: SP, RJ
        public string CEP { get; set; } = string.Empty; // 8 dígitos
        public bool Ativo { get; set; } = true;
        public string? CodigoIBGE { get; set; }
        public bool Capital { get; set; } = false;

        // Relacionamento: Estado opcional
        public int EstadoId { get; set; }
        public virtual Estado? Estado { get; set; }
    }
}
