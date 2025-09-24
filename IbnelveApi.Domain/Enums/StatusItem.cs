namespace IbnelveApi.Domain.Enums;

/// <summary>
/// Status possíveis para um item no sistema.
/// </summary>
public enum StatusItem
{
    /// <summary>
    /// Item disponível para uso.
    /// </summary>
    Ativo = 1,

    /// <summary>
    /// Item está emprestado para um usuário.
    /// </summary>
    Emprestado = 2,

    /// <summary>
    /// Item está reservado e não pode ser emprestado.
    /// </summary>
    Reservado = 3,

    /// <summary>
    /// Item está em trânsito entre locais.
    /// </summary>
    EmTransito = 4,

    /// <summary>
    /// Item foi descartado e não está mais disponível.
    /// </summary>
    Descartado = 5,

    /// <summary>
    /// Item está em manutenção e indisponível.
    /// </summary>
    EmManutencao = 6
}