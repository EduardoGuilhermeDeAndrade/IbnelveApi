// Enum de papéis do sistema. Mantido no domínio, sem dependências externas.
namespace IbnelveApi.Domain.Enums
{
    /// <summary>
    /// Papéis de acesso do sistema.
    /// </summary>
    public enum Role
    {
        Admin = 1,
        Secretario = 2,
        User = 3
    }
}