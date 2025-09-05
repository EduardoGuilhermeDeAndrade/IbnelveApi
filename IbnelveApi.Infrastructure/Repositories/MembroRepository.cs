using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IbnelveApi.Infrastructure.Repositories;

public class MembroRepository : TenantRepository<Membro>, IMembroRepository
{
    public MembroRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Membro?> GetByCpfAsync(string cpf, string tenantId)
    {
        return await ApplyTenantFilter(tenantId)
            .Where(p => p.CPF == cpf)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Membro>> GetByNomeAsync(string nome, string tenantId)
    {
        return await ApplyTenantFilter(tenantId)
            .Where(p => p.Nome.Contains(nome))
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<bool> CpfExistsAsync(string cpf, string tenantId, int? excludeId = null)
    {
        var query = ApplyTenantFilter(tenantId)
            .Where(p => p.CPF == cpf);

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Membro>> SearchAsync(string searchTerm, string tenantId, bool includeDeleted = false)
    {
        var termLower = searchTerm.ToLower();

        return await ApplyTenantFilter(tenantId, includeDeleted)
            .Where(p => p.Nome.ToLower().Contains(termLower) ||
                       p.CPF.Contains(searchTerm) ||
                       p.Telefone.Contains(searchTerm) ||
                       p.Endereco.Cidade.ToLower().Contains(termLower))
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Membro>> GetWithFiltersAsync(
        string tenantId,
        string? nome = null,
        string? cpf = null,
        string? cidade = null,
        string? uf = null,
        bool includeDeleted = false,
        string orderBy = "Nome")
    {
        var query = ApplyTenantFilter(tenantId, includeDeleted);

        // Aplicar filtros opcionais
        if (!string.IsNullOrEmpty(nome))
            query = query.Where(p => p.Nome.Contains(nome));

        if (!string.IsNullOrEmpty(cpf))
            query = query.Where(p => p.CPF.Contains(cpf));

        if (!string.IsNullOrEmpty(cidade))
            query = query.Where(p => p.Endereco.Cidade.Contains(cidade));

        if (!string.IsNullOrEmpty(uf))
            query = query.Where(p => p.Endereco.UF == uf);

        // Aplicar ordenação
        query = orderBy.ToLower() switch
        {
            "nome" => query.OrderBy(p => p.Nome),
            "cpf" => query.OrderBy(p => p.CPF),
            "cidade" => query.OrderBy(p => p.Endereco.Cidade),
            "createdat" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderBy(p => p.Nome)
        };

        return await query.ToListAsync();
    }
}

