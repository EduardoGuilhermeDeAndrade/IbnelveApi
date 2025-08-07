// IbnelveApi.Infrastructure/Repositories/PessoaRepository.cs
using Microsoft.EntityFrameworkCore;
using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using IbnelveApi.Infrastructure.Services;

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

    //public async Task<IEnumerable<Pessoa>> GetByNomeIncludingDeletedAsync(string nome)
    //{
    //    // Incluir deletados mas manter filtro de tenant
    //    return await _dbSet.IgnoreQueryFilters()
    //        .Where(p => p.Nome.Contains(nome) &&
    //                   (tenantContext.TenantId == null || p.TenantId == _context.TenantContext.TenantId))
    //        .ToListAsync();
    //}

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

//using Microsoft.EntityFrameworkCore;
//using IbnelveApi.Domain.Entities;
//using IbnelveApi.Domain.Interfaces;
//using IbnelveApi.Infrastructure.Data;

//namespace IbnelveApi.Infrastructure.Repositories;

//public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
//{
//    public PessoaRepository(ApplicationDbContext context) : base(context)
//    {
//    }

//    public async Task<Pessoa?> GetByCpfAsync(string cpf, string tenantId)
//    {
//        return await _dbSet
//            .Where(p => p.CPF == cpf && p.TenantId == tenantId)
//            .FirstOrDefaultAsync();
//    }

//    public async Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome, string tenantId, bool includeDeleted = false)
//    {
//        var query = _dbSet.Where(p => p.Nome.Contains(nome) && p.TenantId == tenantId);

//        if (includeDeleted)
//        {
//            query = query.IgnoreQueryFilters().Where(p => p.TenantId == tenantId && p.Nome.Contains(nome));
//        }

//        return await query.ToListAsync();
//    }

//    public async Task<bool> CpfExistsAsync(string cpf, string tenantId, int? excludeId = null)
//    {
//        var query = _dbSet.IgnoreQueryFilters()
//            .Where(p => p.CPF == cpf && p.TenantId == tenantId && !p.IsDeleted);

//        if (excludeId.HasValue)
//        {
//            query = query.Where(p => p.Id != excludeId.Value);
//        }

//        return await query.AnyAsync();
//    }
//}

