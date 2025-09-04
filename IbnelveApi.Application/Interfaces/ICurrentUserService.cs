namespace IbnelveApi.Application.Interfaces;

/// <summary>
/// Interface para capturar informações do usuário atual autenticado
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Obtém o ID do usuário atual
    /// </summary>
    string GetUserId();

    /// <summary>
    /// Obtém o ID do tenant do usuário atual
    /// </summary>
    string GetTenantId();

    /// <summary>
    /// Obtém o nome do usuário atual
    /// </summary>
    string GetUserName();

    /// <summary>
    /// Verifica se o usuário está autenticado
    /// </summary>
    bool IsAuthenticated();
}

