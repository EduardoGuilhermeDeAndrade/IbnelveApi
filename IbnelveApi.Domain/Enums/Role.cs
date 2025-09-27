// Enum de pap�is do sistema. Mantido no dom�nio, sem depend�ncias externas.
namespace IbnelveApi.Domain.Enums
{
    /// <summary>
    /// Pap�is de acesso do sistema.
    /// </summary>
    public enum Role
    {
        Admin = 1,
        Secretario = 2,
        User = 3
    }
}