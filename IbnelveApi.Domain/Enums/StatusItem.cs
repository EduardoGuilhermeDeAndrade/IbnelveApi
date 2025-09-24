namespace IbnelveApi.Domain.Enums;

/// <summary>
/// Status poss�veis para um item no sistema.
/// </summary>
public enum StatusItem
{
    /// <summary>
    /// Item dispon�vel para uso.
    /// </summary>
    Ativo = 1,

    /// <summary>
    /// Item est� emprestado para um usu�rio.
    /// </summary>
    Emprestado = 2,

    /// <summary>
    /// Item est� reservado e n�o pode ser emprestado.
    /// </summary>
    Reservado = 3,

    /// <summary>
    /// Item est� em tr�nsito entre locais.
    /// </summary>
    EmTransito = 4,

    /// <summary>
    /// Item foi descartado e n�o est� mais dispon�vel.
    /// </summary>
    Descartado = 5,

    /// <summary>
    /// Item est� em manuten��o e indispon�vel.
    /// </summary>
    EmManutencao = 6
}