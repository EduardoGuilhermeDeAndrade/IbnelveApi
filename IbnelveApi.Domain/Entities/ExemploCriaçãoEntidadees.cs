//namespace IbnelveApi.Domain.Entities;

///// <summary>
///// Entidade global - País
///// Compartilhada entre TODOS os tenants
///// </summary>
//public class Pais : GlobalEntity
//{
//    public string Nome { get; set; } = string.Empty;
//    public string CodigoISO2 { get; set; } = string.Empty; // BR, US, AR
//    public string CodigoISO3 { get; set; } = string.Empty; // BRA, USA, ARG
//    public string? CodigoTelefone { get; set; } // +55, +1, +54
//    public bool Ativo { get; set; } = true;

//    // Relacionamentos
//    public virtual ICollection<Estado> Estados { get; set; } = new List<Estado>();

//    public Pais() { }

//    public Pais(string nome, string codigoISO2, string codigoISO3, string? codigoTelefone = null)
//    {
//        Nome = nome;
//        CodigoISO2 = codigoISO2;
//        CodigoISO3 = codigoISO3;
//        CodigoTelefone = codigoTelefone;
//    }
//}

///// <summary>
///// Entidade global - Estado/Província
///// Compartilhada entre TODOS os tenants
///// </summary>
//public class Estado : GlobalEntity
//{
//    public string Nome { get; set; } = string.Empty;
//    public string Sigla { get; set; } = string.Empty; // SP, RJ, MG
//    public int PaisId { get; set; }
//    public bool Ativo { get; set; } = true;

//    // Relacionamentos
//    public virtual Pais Pais { get; set; } = null!;
//    public virtual ICollection<Cidade> Cidades { get; set; } = new List<Cidade>();

//    public Estado() { }

//    public Estado(string nome, string sigla, int paisId)
//    {
//        Nome = nome;
//        Sigla = sigla;
//        PaisId = paisId;
//    }
//}

///// <summary>
///// Entidade global - Cidade
///// Compartilhada entre TODOS os tenants
///// </summary>
//public class Cidade : GlobalEntity
//{
//    public string Nome { get; set; } = string.Empty;
//    public int EstadoId { get; set; }
//    public string? CodigoIBGE { get; set; } // Para cidades brasileiras
//    public bool Capital { get; set; } = false;
//    public bool Ativo { get; set; } = true;

//    // Relacionamentos
//    public virtual Estado Estado { get; set; } = null!;

//    public Cidade() { }

//    public Cidade(string nome, int estadoId, string? codigoIBGE = null, bool capital = false)
//    {
//        Nome = nome;
//        EstadoId = estadoId;
//        CodigoIBGE = codigoIBGE;
//        Capital = capital;
//    }
//}




//namespace IbnelveApi.Domain.Entities;

/// <summary>
/// Entidade do tenant - Categoria de Tarefas
/// Herda de TenantEntity pois é compartilhada entre todos os usuários do tenant
/// </summary>
//public class CategoriaTarefa : TenantEntity
//{
//    public string Nome { get; set; } = string.Empty;
//    public string? Descricao { get; set; }
//    public string? Cor { get; set; } // Para exibição visual (#FFFFFF)
//    public bool Ativa { get; set; } = true;

//    public CategoriaTarefa() { }

//    public CategoriaTarefa(string nome, string tenantId, string? descricao = null, string? cor = null)
//    {
//        Nome = nome;
//        TenantId = tenantId;
//        Descricao = descricao;
//        Cor = cor;
//    }
//}

///// <summary>
///// Entidade do tenant - Configurações do Tenant
///// Herda de TenantEntity pois é compartilhada entre todos os usuários do tenant
///// </summary>
//public class ConfiguracaoTenant : TenantEntity
//{
//    public string Chave { get; set; } = string.Empty;
//    public string Valor { get; set; } = string.Empty;
//    public string? Descricao { get; set; }
//    public string Tipo { get; set; } = "string"; // string, int, bool, json, etc.

//    public ConfiguracaoTenant() { }

//    public ConfiguracaoTenant(string chave, string valor, string tenantId, string? descricao = null, string tipo = "string")
//    {
//        Chave = chave;
//        Valor = valor;
//        TenantId = tenantId;
//        Descricao = descricao;
//        Tipo = tipo;
//    }
//}

///// <summary>
///// Entidade do tenant - Departamentos/Setores
///// Herda de TenantEntity pois é compartilhada entre todos os usuários do tenant
///// </summary>
//public class Departamento : TenantEntity
//{
//    public string Nome { get; set; } = string.Empty;
//    public string? Descricao { get; set; }
//    public bool Ativo { get; set; } = true;

//    public Departamento() { }

//    public Departamento(string nome, string tenantId, string? descricao = null)
//    {
//        Nome = nome;
//        TenantId = tenantId;
//        Descricao = descricao;
//    }
//}

