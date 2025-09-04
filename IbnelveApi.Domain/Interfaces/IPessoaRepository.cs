using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Pessoa
/// ATUALIZADA: Mantém métodos originais + versões com tenantId
/// </summary>
public interface IPessoaRepository : IRepository<Pessoa>
{
    // ===== MÉTODOS ORIGINAIS (mantidos para compatibilidade) =====
    Task<Pessoa?> GetByCpfAsync(string cpf);
    Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome);
    Task<bool> CpfExistsAsync(string cpf, int? excludeId = null);

    // ===== NOVOS MÉTODOS COM FILTRO POR TENANT (opcionais) =====
    //Task<Pessoa?> GetByCpfAsync(string cpf, string tenantId);
    //Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome, string tenantId);
    //Task<bool> CpfExistsAsync(string cpf, string tenantId, int? excludeId = null);
}

