// IbnelveApi.Domain/Interfaces/IPessoaRepository.cs
using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

public interface IPessoaRepository : IRepository<Pessoa>
{
    Task<Pessoa?> GetByCpfAsync(string cpf);
    Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome);
    //Task<IEnumerable<Pessoa>> GetByNomeIncludingDeletedAsync(string nome);
    Task<bool> CpfExistsAsync(string cpf, int? excludeId = null);
}
