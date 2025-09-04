using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IbnelveApi.Infrastructure.Repositories;

/// <summary>
/// Repositório para Pessoa - VERSÃO SIMPLES
/// CORRIGIDO: Substitui _dbSet por _context.Set<Pessoa>()
/// </summary>
public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
{
    public PessoaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Pessoa?> GetByCpfAsync(string cpf)
    {
        //  CORRIGIDO: _dbSet  _context.Set<Pessoa>()
        return await _context.Set<Pessoa>()
            .Where(p => p.CPF == cpf)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome)
    {
        //  CORRIGIDO: _dbSet  _context.Set<Pessoa>()
        return await _context.Set<Pessoa>()
            .Where(p => p.Nome.Contains(nome))
            .ToListAsync();
    }

    public async Task<bool> CpfExistsAsync(string cpf, int? excludeId = null)
    {
        //  CORRIGIDO: _dbSet  _context.Set<Pessoa>()
        var query = _context.Set<Pessoa>().Where(p => p.CPF == cpf);

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}

