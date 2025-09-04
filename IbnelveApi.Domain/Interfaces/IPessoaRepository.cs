using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

/// <summary>
/// Interface do reposit�rio de Pessoa
/// ATUALIZADA: Mant�m m�todos originais + vers�es com tenantId
/// </summary>
public interface IPessoaRepository : IRepository<Pessoa>
{
    // ===== M�TODOS ORIGINAIS (mantidos para compatibilidade) =====
    Task<Pessoa?> GetByCpfAsync(string cpf);
    Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome);
    Task<bool> CpfExistsAsync(string cpf, int? excludeId = null);

    // ===== NOVOS M�TODOS COM FILTRO POR TENANT (opcionais) =====
    //Task<Pessoa?> GetByCpfAsync(string cpf, string tenantId);
    //Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome, string tenantId);
    //Task<bool> CpfExistsAsync(string cpf, string tenantId, int? excludeId = null);
}

