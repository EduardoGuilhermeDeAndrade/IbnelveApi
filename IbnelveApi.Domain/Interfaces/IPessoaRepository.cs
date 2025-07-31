using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

public interface IPessoaRepository : IRepository<Pessoa>
{
    Task<Pessoa?> GetByCpfAsync(string cpf, string tenantId);
    Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome, string tenantId, bool includeDeleted = false);
    Task<bool> CpfExistsAsync(string cpf, string tenantId, int? excludeId = null);
}

