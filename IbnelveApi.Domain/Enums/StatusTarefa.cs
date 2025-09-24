namespace IbnelveApi.Domain.Enums;

/// <summary>
/// Status possíveis para uma tarefa.
/// </summary>
public enum StatusTarefa
{
    /// <summary>
    /// Tarefa criada, aguardando início.
    /// </summary>
    Pendente = 1,

    /// <summary>
    /// Tarefa em andamento, sendo executada.
    /// </summary>
    EmAndamento = 2,

    /// <summary>
    /// Tarefa concluída com sucesso.
    /// </summary>
    Concluida = 3,

    /// <summary>
    /// Tarefa cancelada antes da conclusão.
    /// </summary>
    Cancelada = 4
}

