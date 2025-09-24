using IbnelveApi.Domain.Entities;
using IbnelveApi.Domain.Interfaces;
using IbnelveApi.Infrastructure.Data;

namespace IbnelveApi.Infrastructure.Repositories;

public class UtensilioRepository : TenantRepository<Utensilio>, IUtensilioRepository
{
    public UtensilioRepository(ApplicationDbContext context) : base(context)
    {
    }
}