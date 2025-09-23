using IbnelveApi.Domain.Entities.General;
using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Domain.Entities;

/// <summary>
/// Entidade Tarefa - herda de UserOwnedEntity pois tarefas s�o espec�ficas do usu�rio
/// </summary>
public class Tarefa : UserOwnedEntity
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
    public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;
    public DateTime? DataVencimento { get; set; }
    public DateTime? DataConclusao { get; set; }
    //public string? Categoria { get; set; }
    public int? CategoriaId { get; set; }
    public virtual CategoriaTarefa? Categoria { get; set; }

    public Tarefa() { }

    public Tarefa(string titulo, string descricao, PrioridadeTarefa prioridade, DateTime? dataVencimento,
              int? categoriaId, string tenantId, string userId)
    {
        Titulo = titulo;
        Descricao = descricao;
        Prioridade = prioridade;
        DataVencimento = dataVencimento;
        CategoriaId = categoriaId; // NOVA LINHA
        TenantId = tenantId;
        UserId = userId;
        Status = StatusTarefa.Pendente;
    }

    public void AtualizarDados(string titulo, string descricao, PrioridadeTarefa prioridade, DateTime? dataVencimento, int? categoriaId)
    {
        Titulo = titulo;
        Descricao = descricao;
        Prioridade = prioridade;
        DataVencimento = dataVencimento;
        CategoriaId = categoriaId; // NOVA LINHA
        UpdatedAt = DateTime.UtcNow;
    }

    public void AlterarStatus(StatusTarefa novoStatus)
    {
        Status = novoStatus;

        if (novoStatus == StatusTarefa.Concluida)
        {
            DataConclusao = DateTime.UtcNow;
        }
        else if (DataConclusao.HasValue && novoStatus != StatusTarefa.Concluida)
        {
            DataConclusao = null;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void MarcarComoConcluida()
    {
        AlterarStatus(StatusTarefa.Concluida);
    }

    public void MarcarComoPendente()
    {
        AlterarStatus(StatusTarefa.Pendente);
    }

    public void MarcarComoEmAndamento()
    {
        AlterarStatus(StatusTarefa.EmAndamento);
    }

    public void Cancelar()
    {
        AlterarStatus(StatusTarefa.Cancelada);
    }

    public bool EstaVencida()
    {
        return DataVencimento.HasValue &&
               DataVencimento.Value.Date < DateTime.UtcNow.Date &&
               Status != StatusTarefa.Concluida;
    }

    public bool EstaConcluida()
    {
        return Status == StatusTarefa.Concluida;
    }

    public bool EstaPendente()
    {
        return Status == StatusTarefa.Pendente;
    }
}

