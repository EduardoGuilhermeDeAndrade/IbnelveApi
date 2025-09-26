using IbnelveApi.Domain.Entities;

namespace IbnelveApi.Domain.Interfaces;

/// <summary>
/// Interface do repositório para Utensilio.
/// </summary>
public interface IUtensilioRepository : ITenantRepository<Utensilio>
{
    // Métodos específicos podem ser adicionados aqui.
    //Task<bool> EstaSendoUsadaAsync(Guid id, string tenantId);
}