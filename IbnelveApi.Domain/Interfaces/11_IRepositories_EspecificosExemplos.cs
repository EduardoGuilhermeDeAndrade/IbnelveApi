//using IbnelveApi.Domain.Entities;
//using IbnelveApi.Domain.Enums;

//namespace IbnelveApi.Domain.Interfaces;

//// ===== INTERFACES ESPECÍFICAS PARA ENTIDADES GLOBAIS =====

///// <summary>
///// Interface para repositório de Países (entidade global)
///// </summary>
//public interface IPaisRepository : IGlobalRepository<Pais>
//{
//    Task<Pais?> GetByCodigoISO2Async(string codigoISO2);
//    Task<Pais?> GetByCodigoISO3Async(string codigoISO3);
//    Task<IEnumerable<Pais>> GetWithEstadosAsync();
//}

///// <summary>
///// Interface para repositório de Estados (entidade global)
///// </summary>
//public interface IEstadoRepository : IGlobalRepository<Estado>
//{
//    Task<IEnumerable<Estado>> GetByPaisAsync(int paisId);
//    Task<Estado?> GetBySiglaAsync(string sigla, int paisId);
//    Task<IEnumerable<Estado>> GetWithCidadesAsync(int paisId);
//}

///// <summary>
///// Interface para repositório de Cidades (entidade global)
///// </summary>
//public interface ICidadeRepository : IGlobalRepository<Cidade>
//{
//    Task<IEnumerable<Cidade>> GetByEstadoAsync(int estadoId);
//    Task<IEnumerable<Cidade>> GetByPaisAsync(int paisId);
//    Task<Cidade?> GetByCodigoIBGEAsync(string codigoIBGE);
//    Task<IEnumerable<Cidade>> GetCapitaisAsync();
//    Task<IEnumerable<Cidade>> SearchByNomeAsync(string nome);
//}

//// ===== INTERFACES ESPECÍFICAS PARA ENTIDADES DO TENANT =====

///// <summary>
///// Interface para repositório de Categorias (entidade do tenant)
///// </summary>
//public interface ICategoriaRepository : ITenantRepository<CategoriaTarefa>
//{
//    Task<IEnumerable<CategoriaTarefa>> GetAtivasAsync(string tenantId);
//    Task<CategoriaTarefa?> GetByNomeAsync(string nome, string tenantId);
//}

///// <summary>
///// Interface para repositório de Configurações (entidade do tenant)
///// </summary>
//public interface IConfiguracaoRepository : ITenantRepository<ConfiguracaoTenant>
//{
//    Task<ConfiguracaoTenant?> GetByChaveAsync(string chave, string tenantId);
//    Task<IEnumerable<ConfiguracaoTenant>> GetByTipoAsync(string tipo, string tenantId);
//}



