namespace IbnelveApi.Domain.Enums;

/// <summary>
/// Níveis de prioridade para tarefas.
/// </summary>
public enum PrioridadeTarefa
{
    /// <summary>
    /// Prioridade baixa: tarefas sem urgência.
    /// </summary>
    Baixa = 1,

    /// <summary>
    /// Prioridade média: tarefas importantes, mas não críticas.
    /// </summary>
    Media = 2,

    /// <summary>
    /// Prioridade alta: tarefas urgentes que devem ser tratadas rapidamente.
    /// </summary>
    Alta = 3,

    /// <summary>
    /// Prioridade crítica: tarefas que exigem atenção imediata.
    /// </summary>
    Critica = 4
}

