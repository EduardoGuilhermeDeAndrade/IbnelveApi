using IbnelveApi.Domain.Entities;
using IbnelveApi.Infrastructure.Data;
using IbnelveApi.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IbnelveApi.Infrastructure.Repositories;

public class CidadeRepository : ICidadeRepository
{
    private readonly ApplicationDbContext _context;

    public CidadeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cidade>> GetAllAsync(bool includeDeleted = false)
    {
        return await _context.Cidades
            .Where(c => includeDeleted || !c.IsDeleted)
            .Include(c => c.Estado)
            .ToListAsync();
    }

    public async Task<Cidade?> GetByIdAsync(int id)
    {
        return await _context.Cidades
            .Include(c => c.Estado)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Cidade cidade)
    {
        _context.Cidades.Add(cidade);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cidade cidade)
    {
        _context.Cidades.Update(cidade);
        await _context.SaveChangesAsync();
    }
}
