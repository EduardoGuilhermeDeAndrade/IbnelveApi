
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IbnelveApi.Infrastructure.Repositories;

public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
{
    public PessoaRepository(ApplicationDbContext context ) : base(context)
    {
    }

    public async Task<Pessoa?> GetByCpfAsync(string cpf)
    {
        // Global filters aplicados automaticamente - sem necessidade de filtrar tenant
        return await _dbSet
            .Where(p => p.CPF == cpf)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome)
    {
        // Global filters aplicados automaticamente - sem necessidade de filtrar tenant
        return await _dbSet
            .Where(p => p.Nome.Contains(nome))
            .ToListAsync();
    }

    public async Task<bool> CpfExistsAsync(string cpf, int? excludeId = null)
    {
        // Global filters aplicados automaticamente
        var query = _dbSet.Where(p => p.CPF == cpf);

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}



