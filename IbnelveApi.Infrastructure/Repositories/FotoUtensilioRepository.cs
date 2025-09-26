using IbnelveApi.Infrastructure.Data;
using IbnelveApi.Infrastructure.Repositories;

public class FotoUtensilioRepository : TenantRepository<FotoUtensilio>, IFotoUtensilioRepository
{
    public FotoUtensilioRepository(ApplicationDbContext context) : base(context) { }
}