namespace IbnelveApi.Domain.Enums;

/// <summary>
/// N�veis de prioridade para tarefas.
/// </summary>
public enum PrioridadeTarefa
{
    /// <summary>
    /// Prioridade baixa: tarefas sem urg�ncia.
    /// </summary>
    Baixa = 1,

    /// <summary>
    /// Prioridade m�dia: tarefas importantes, mas n�o cr�ticas.
    /// </summary>
    Media = 2,

    /// <summary>
    /// Prioridade alta: tarefas urgentes que devem ser tratadas rapidamente.
    /// </summary>
    Alta = 3,

    /// <summary>
    /// Prioridade cr�tica: tarefas que exigem aten��o imediata.
    /// </summary>
    Critica = 4
}

