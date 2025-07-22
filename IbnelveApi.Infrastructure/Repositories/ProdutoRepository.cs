using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;

namespace IbnelveApi.Infrastructure.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Produto>> GetByNomeAsync(string nome)
    {
        return await _dbSet
            .Where(p => p.Nome.Contains(nome))
            .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> GetByPrecoRangeAsync(decimal precoMin, decimal precoMax)
    {
        return await _dbSet
            .Where(p => p.Preco >= precoMin && p.Preco <= precoMax)
            .ToListAsync();
    }
}

