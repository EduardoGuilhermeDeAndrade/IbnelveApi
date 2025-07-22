using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> GetByNomeAsync(string nome);
    Task<IEnumerable<Produto>> GetByPrecoRangeAsync(decimal precoMin, decimal precoMax);
}

