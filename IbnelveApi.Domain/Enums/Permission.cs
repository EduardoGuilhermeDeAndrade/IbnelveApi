// Enum de permiss�es do sistema. Cada valor representa uma a��o/tela. N�o referencia pacotes externos.
namespace IbnelveApi.Domain.Enums
{
    /// <summary>
    /// Permiss�es do sistema, representando a��es/telas. Este enum N�O deve referenciar pacotes externos.
    /// </summary>
    public enum Permission
    {
        Pessoa_Read,
        Pessoa_Create,
        Pessoa_Update,
        Pessoa_Delete,
        Tarefa_Read,
        Tarefa_Create,
        Tarefa_Update,
        Tarefa_Delete,
        Relatorio_View,
        Configuracao_Edit
        // Adicione outras permiss�es conforme necess�rio
    }
}