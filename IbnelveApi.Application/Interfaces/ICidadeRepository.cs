using IbnelveApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IbnelveApi.Application.Interfaces;

public interface ICidadeRepository
{
    Task<IEnumerable<Cidade>> GetAllAsync(bool includeDeleted = false);
    Task<Cidade?> GetByIdAsync(int id);
    Task AddAsync(Cidade cidade);
    Task UpdateAsync(Cidade cidade);
}
