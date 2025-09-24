namespace IbnelveApi.Domain.Enums;

/// <summary>
/// Status poss�veis para uma tarefa.
/// </summary>
public enum StatusTarefa
{
    /// <summary>
    /// Tarefa criada, aguardando in�cio.
    /// </summary>
    Pendente = 1,

    /// <summary>
    /// Tarefa em andamento, sendo executada.
    /// </summary>
    EmAndamento = 2,

    /// <summary>
    /// Tarefa conclu�da com sucesso.
    /// </summary>
    Concluida = 3,

    /// <summary>
    /// Tarefa cancelada antes da conclus�o.
    /// </summary>
    Cancelada = 4
}

