using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Entities;

/// <summary>
/// Entidade CategoriaTarefa - representa uma categoria de tarefa no sistema
/// Herda de TenantEntity para suporte a multi-tenancy
/// </summary>
public class CategoriaTarefa : TenantEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Cor { get; set; } // Cor hexadecimal para UI (ex: #FF5733)
    public bool Ativa { get; set; } = true;

    // Relacionamento com Tarefas
    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();

    public CategoriaTarefa() { }

    public CategoriaTarefa(string nome, string? descricao, string? cor, string tenantId, string userId)
    {
        Nome = nome;
        Descricao = descricao;
        Cor = cor;
        TenantId = tenantId;
        Ativa = true;
    }

    public void AtualizarDados(string nome, string? descricao, string? cor)
    {
        Nome = nome;
        Descricao = descricao;
        Cor = cor;
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

