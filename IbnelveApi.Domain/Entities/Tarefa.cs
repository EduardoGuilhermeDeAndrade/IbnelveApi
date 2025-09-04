using IbnelveApi.Domain.Enums;

namespace IbnelveApi.Domain.Entities;

/// <summary>
/// Entidade Tarefa - herda de UserOwnedEntity pois tarefas são específicas do usuário
/// </summary>
public class Tarefa : UserOwnedEntity
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
    public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;
    public DateTime? DataVencimento { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string? Categoria { get; set; }

    public Tarefa() { }

    public Tarefa(string titulo, string descricao, string userId, string tenantId,
                  PrioridadeTarefa prioridade = PrioridadeTarefa.Media,
                  DateTime? dataVencimento = null, string? categoria = null)
    {
        Titulo = titulo;
        Descricao = descricao;
        UserId = userId;        //  Específico do usuário
        TenantId = tenantId;    //  Específico do tenant
        Prioridade = prioridade;
        DataVencimento = dataVencimento;
        Categoria = categoria;
        Status = StatusTarefa.Pendente;
    }

    public void AtualizarDados(string titulo, string descricao, PrioridadeTarefa prioridade, DateTime? dataVencimento = null, string? categoria = null)
    {
        Titulo = titulo;
        Descricao = descricao;
        Prioridade = prioridade;
        DataVencimento = dataVencimento;
        Categoria = categoria;
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

