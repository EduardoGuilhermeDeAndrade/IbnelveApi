using IbnelveApi.Domain.Entities.General;
using System.Runtime.ConstrainedExecution;

namespace IbnelveApi.Domain.Entities
{
    /// <summary>
    /// Representa uma categoria de utensílios.
    /// </summary>
    public class CategoriaUtensilio : TenantEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativa { get; set; } = true;
        // Navegação reversa (opcional)
        public ICollection<Utensilio> Utensilios { get; set; } = new List<Utensilio>();


        public CategoriaUtensilio() { } // Construtor padrão necessário para EF Core

        public CategoriaUtensilio(string nome, string descricao, string tenantId, string userId)
        {
            Nome = nome;
            Descricao = descricao;
            TenantId = tenantId;
            // userId não deve ser obrigatório para o EF Core
        }

        public void AtualizarDados(string nome, string? descricao)
        {
            Nome = nome;
            Descricao = descricao;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Ativar()
        {
            Ativa = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Desativar()
        {
            Ativa = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}