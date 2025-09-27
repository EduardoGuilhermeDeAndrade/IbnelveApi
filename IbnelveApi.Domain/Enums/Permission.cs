// Enum de permissões do sistema. Cada valor representa uma ação/tela. Não referencia pacotes externos.
namespace IbnelveApi.Domain.Enums
{
    /// <summary>
    /// Permissões do sistema, representando ações/telas. Este enum NÃO deve referenciar pacotes externos.
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
        // Adicione outras permissões conforme necessário
    }
}