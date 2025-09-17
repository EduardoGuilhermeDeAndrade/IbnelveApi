# Model Context Protocol (MCP) - IbnelveApi

## 📋 Visão Geral do Projeto

**IbnelveApi** é uma API RESTful desenvolvida com .NET 9 seguindo os princípios da Clean Architecture. O projeto implementa um sistema de gestão com suporte a multi-tenancy, autenticação JWT, soft delete e funcionalidades CRUD completas.

### 🏗️ Arquitetura Clean Architecture

O projeto está organizado em 6 camadas bem definidas:

```
IbnelveApi/
├── IbnelveApi.Api/           # 🌐 Camada de Apresentação
├── IbnelveApi.Application/   # 📋 Camada de Aplicação  
├── IbnelveApi.Domain/        # 🏛️ Camada de Domínio
├── IbnelveApi.Infrastructure/# 🔧 Camada de Infraestrutura
├── IbnelveApi.IoC/          # 🔌 Injeção de Dependência
└── IbnelveApi.Tests/        # 🧪 Testes
```

---

## 🚀 Tecnologias e Ferramentas

### Framework e Runtime
- **.NET 9**: Framework principal
- **ASP.NET Core**: Web API
- **C# 12**: Linguagem de programação

### Persistência de Dados
- **Entity Framework Core 9.0.7**: ORM
- **SQL Server**: Banco de dados principal
- **ASP.NET Identity**: Sistema de autenticação

### Autenticação e Segurança
- **JWT Bearer**: Tokens de autenticação
- **Multi-tenancy**: Isolamento por TenantId
- **Soft Delete**: Exclusão lógica

### Validação e Documentação
- **FluentValidation 11.3.1**: Validação de dados
- **Swagger/OpenAPI**: Documentação interativa
- **Swashbuckle.AspNetCore 9.0.3**: Geração de documentação

### Padrões e Práticas
- **Clean Architecture**: Separação de responsabilidades
- **Repository Pattern**: Acesso a dados
- **Service Layer**: Lógica de negócio
- **DTO Pattern**: Transferência de dados

---

## 🏛️ Camada de Domínio (Domain)

### 📁 Estrutura
```
IbnelveApi.Domain/
├── Entities/           # Entidades do domínio
├── Enums/             # Enumerações
├── Extensions/        # Métodos de extensão
├── Interfaces/        # Contratos do domínio
└── ValueObjects/      # Objetos de valor
```

### 🏗️ Hierarquia de Entidades

```csharp
// Hierarquia base das entidades
GlobalEntity          // Entidade global (sem tenant/user)
├── TenantEntity     // Entidade por tenant
│   └── BaseEntity   // (Alias para compatibilidade)
└── UserOwnedEntity  // Entidade por usuário
```

### 📝 Padrão de Entidades

#### GlobalEntity (Base Universal)
```csharp
public abstract class GlobalEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual void ExcluirLogicamente()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

#### TenantEntity (Dados por Tenant)
```csharp
public abstract class TenantEntity : GlobalEntity
{
    public string TenantId { get; set; } = string.Empty;
    
    public bool BelongsToTenant(string tenantId) => TenantId == tenantId;
}
```

#### UserOwnedEntity (Dados por Usuário)
```csharp
public abstract class UserOwnedEntity : TenantEntity
{
    public string UserId { get; set; } = string.Empty;
    
    public bool BelongsToUser(string userId) => UserId == userId;
}
```

### 🎯 Exemplo de Entidade de Domínio
```csharp
namespace IbnelveApi.Domain.Entities;

public class Membro : UserOwnedEntity
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public Endereco Endereco { get; set; } = new();
    
    // Relacionamentos
    public ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
```

### 🔢 Padrão de Enums
```csharp
namespace IbnelveApi.Domain.Enums;

public enum StatusTarefa
{
    Pendente = 1,
    EmAndamento = 2,
    Concluida = 3,
    Cancelada = 4
}

public enum PrioridadeTarefa
{
    Baixa = 1,
    Media = 2,
    Alta = 3,
    Critica = 4
}
```

---

## 📋 Camada de Aplicação (Application)

### 📁 Estrutura
```
IbnelveApi.Application/
├── Common/           # Respostas comuns (ApiResponse)
├── DTOs/            # Data Transfer Objects
├── Extensions/      # Métodos de extensão
├── Interfaces/      # Contratos de serviços
├── Mappings/        # Mapeamentos manuais
├── Services/        # Lógica de aplicação
└── Validators/      # Validações FluentValidation
```

### 🔄 Padrão de DTOs

#### DTO de Resposta (Read)
```csharp
namespace IbnelveApi.Application.DTOs;

public class MembroDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public EnderecoDto Endereco { get; set; } = new();
    public string TenantId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

#### DTO de Criação (Create)
```csharp
public class CreateMembroDto
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public CreateEnderecoDto Endereco { get; set; } = new();
}
```

#### DTO de Atualização (Update)
```csharp
public class UpdateMembroDto
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public UpdateEnderecoDto Endereco { get; set; } = new();
}
```

### 🔧 Padrão de Serviços

#### Interface do Serviço
```csharp
namespace IbnelveApi.Application.Interfaces;

public interface IMembroService
{
    Task<ApiResponse<IEnumerable<MembroDto>>> GetAllAsync(
        string tenantId, 
        string userId, 
        bool includeDeleted = false);
    
    Task<ApiResponse<MembroDto>> GetByIdAsync(
        int id, 
        string tenantId, 
        string userId);
    
    Task<ApiResponse<MembroDto>> CreateAsync(
        CreateMembroDto dto, 
        string tenantId, 
        string userId);
    
    Task<ApiResponse<MembroDto>> UpdateAsync(
        int id, 
        UpdateMembroDto dto, 
        string tenantId, 
        string userId);
    
    Task<ApiResponse<bool>> DeleteAsync(
        int id, 
        string tenantId, 
        string userId);
}
```

#### Implementação do Serviço
```csharp
namespace IbnelveApi.Application.Services;

public class MembroService : IMembroService
{
    private readonly IMembroRepository _repository;
    private readonly IValidator<CreateMembroDto> _createValidator;
    private readonly IValidator<UpdateMembroDto> _updateValidator;

    public MembroService(
        IMembroRepository repository,
        IValidator<CreateMembroDto> createValidator,
        IValidator<UpdateMembroDto> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ApiResponse<MembroDto>> CreateAsync(
        CreateMembroDto dto, 
        string tenantId, 
        string userId)
    {
        // 1. Validação
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return ApiResponse<MembroDto>.ErrorResult(
                "Dados inválidos", 
                validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        // 2. Verificar duplicação (se necessário)
        var existingByCpf = await _repository.GetByCpfAsync(dto.CPF, tenantId, userId);
        if (existingByCpf != null)
        {
            return ApiResponse<MembroDto>.ErrorResult("CPF já cadastrado");
        }

        // 3. Mapear e criar
        var entity = MembroMapping.ToEntity(dto, tenantId, userId);
        var created = await _repository.CreateAsync(entity);

        // 4. Retornar resultado
        var result = MembroMapping.ToDto(created);
        return ApiResponse<MembroDto>.SuccessResult(result, "Membro criado com sucesso");
    }
}
```

### ✅ Padrão de Validações (FluentValidation)

```csharp
namespace IbnelveApi.Application.Validators.Membro;

public class CreateMembroDtoValidator : AbstractValidator<CreateMembroDto>
{
    public CreateMembroDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres")
            .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres")
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("Nome deve conter apenas letras e espaços");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF é obrigatório")
            .Must(cpf => !string.IsNullOrEmpty(cpf?.RemoveSpecialCharacters()))
                .WithMessage("CPF é obrigatório")
            .Must(cpf => cpf.RemoveSpecialCharacters().Length == 11)
                .WithMessage("CPF deve conter exatamente 11 dígitos")
            .Must(cpf => ValidateCPF.BeValidCPF(cpf.RemoveSpecialCharacters()))
                .WithMessage("CPF inválido");

        RuleFor(x => x.Endereco)
            .NotNull().WithMessage("Endereço é obrigatório")
            .SetValidator(new CreateEnderecoDtoValidator());
    }
}
```

### 🔄 Padrão de Mapeamentos

```csharp
namespace IbnelveApi.Application.Mappings;

public static class MembroMapping
{
    public static MembroDto ToDto(Membro entity)
    {
        return new MembroDto
        {
            Id = entity.Id,
            Nome = entity.Nome,
            CPF = entity.CPF,
            Telefone = entity.Telefone,
            Endereco = EnderecoMapping.ToDto(entity.Endereco),
            TenantId = entity.TenantId,
            UserId = entity.UserId,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public static Membro ToEntity(CreateMembroDto dto, string tenantId, string userId)
    {
        return new Membro
        {
            Nome = dto.Nome,
            CPF = dto.CPF.RemoveSpecialCharacters(),
            Telefone = dto.Telefone.RemoveSpecialCharacters(),
            Endereco = EnderecoMapping.ToEntity(dto.Endereco),
            TenantId = tenantId,
            UserId = userId
        };
    }

    public static void UpdateEntity(UpdateMembroDto dto, Membro entity)
    {
        entity.Nome = dto.Nome;
        entity.CPF = dto.CPF.RemoveSpecialCharacters();
        entity.Telefone = dto.Telefone.RemoveSpecialCharacters();
        EnderecoMapping.UpdateEntity(dto.Endereco, entity.Endereco);
        entity.UpdatedAt = DateTime.UtcNow;
    }
}
```

### 📨 Padrão de Resposta da API

```csharp
namespace IbnelveApi.Application.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    // Métodos estáticos para facilitar criação
    public static ApiResponse<T> SuccessResult(T data, string message = "Operação realizada com sucesso")
    {
        return new ApiResponse<T>(data, message);
    }

    public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>(message, errors);
    }
}
```

---

## 🔧 Camada de Infraestrutura (Infrastructure)

### 📁 Estrutura
```
IbnelveApi.Infrastructure/
├── Configurations/    # Configurações EF Core
├── Data/             # DbContext e Seeders
├── Migrations/       # Migrações do banco
└── Repositories/     # Implementações dos repositórios
```

### 🗄️ Padrão de Repositórios

#### Interface Base
```csharp
namespace IbnelveApi.Domain.Interfaces;

public interface IUserOwnedRepository<T> where T : UserOwnedEntity
{
    Task<IEnumerable<T>> GetAllAsync(string tenantId, string userId, bool includeDeleted = false);
    Task<T?> GetByIdAsync(int id, string tenantId, string userId);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id, string tenantId, string userId);
}
```

#### Implementação Base
```csharp
namespace IbnelveApi.Infrastructure.Repositories;

public class UserOwnedRepository<T> : IUserOwnedRepository<T> where T : UserOwnedEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public UserOwnedRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(
        string tenantId, 
        string userId, 
        bool includeDeleted = false)
    {
        var query = _dbSet.AsQueryable();

        if (!includeDeleted)
            query = query.Where(x => !x.IsDeleted);

        return await query
            .Where(x => x.TenantId == tenantId && x.UserId == userId)
            .ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id, string tenantId, string userId)
    {
        return await _dbSet
            .Where(x => x.Id == id && x.TenantId == tenantId && x.UserId == userId && !x.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(int id, string tenantId, string userId)
    {
        var entity = await GetByIdAsync(id, tenantId, userId);
        if (entity == null) return false;

        entity.ExcluirLogicamente();
        await _context.SaveChangesAsync();
        return true;
    }
}
```

### 🏗️ Configuração Entity Framework

```csharp
namespace IbnelveApi.Infrastructure.Configurations;

public class MembroConfiguration : IEntityTypeConfiguration<Membro>
{
    public void Configure(EntityTypeBuilder<Membro> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.CPF)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(m => m.Telefone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.TenantId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.UserId)
            .IsRequired()
            .HasMaxLength(50);

        // Índices para performance
        builder.HasIndex(m => new { m.TenantId, m.UserId, m.CPF })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        // Soft Delete Filter
        builder.HasQueryFilter(m => !m.IsDeleted);

        // Value Object
        builder.OwnsOne(m => m.Endereco, endereco =>
        {
            endereco.Property(e => e.Rua).HasMaxLength(500);
            endereco.Property(e => e.CEP).HasMaxLength(8);
            endereco.Property(e => e.Bairro).HasMaxLength(100);
            endereco.Property(e => e.Cidade).HasMaxLength(100);
            endereco.Property(e => e.UF).HasMaxLength(2);
        });
    }
}
```

---

## 🌐 Camada de Apresentação (Api)

### 📁 Estrutura
```
IbnelveApi.Api/
├── Controllers/       # Controllers da API
├── Middlewares/      # Middlewares customizados
├── Models/           # Models específicos da API
├── Properties/       # Configurações
├── Program.cs        # Configuração da aplicação
└── appsettings.json  # Configurações
```

### 🎮 Padrão de Controllers

```csharp
namespace IbnelveApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembroController : ControllerBase
{
    private readonly IMembroService _service;
    private readonly ICurrentUserService _currentUser;

    public MembroController(IMembroService service, ICurrentUserService currentUser)
    {
        _service = service;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Lista todos os membros do usuário autenticado
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<MembroDto>>>> GetAll(
        [FromQuery] bool includeDeleted = false)
    {
        var tenantId = _currentUser.TenantId;
        var userId = _currentUser.UserId;

        var result = await _service.GetAllAsync(tenantId, userId, includeDeleted);
        return Ok(result);
    }

    /// <summary>
    /// Busca membro por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<MembroDto>>> GetById(int id)
    {
        var tenantId = _currentUser.TenantId;
        var userId = _currentUser.UserId;

        var result = await _service.GetByIdAsync(id, tenantId, userId);
        
        if (!result.Success)
            return NotFound(result);
        
        return Ok(result);
    }

    /// <summary>
    /// Cria novo membro
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<MembroDto>>> Create(
        [FromBody] CreateMembroDto dto)
    {
        var tenantId = _currentUser.TenantId;
        var userId = _currentUser.UserId;

        var result = await _service.CreateAsync(dto, tenantId, userId);
        
        if (!result.Success)
            return BadRequest(result);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Atualiza membro existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<MembroDto>>> Update(
        int id, 
        [FromBody] UpdateMembroDto dto)
    {
        var tenantId = _currentUser.TenantId;
        var userId = _currentUser.UserId;

        var result = await _service.UpdateAsync(id, dto, tenantId, userId);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }

    /// <summary>
    /// Exclui membro (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var tenantId = _currentUser.TenantId;
        var userId = _currentUser.UserId;

        var result = await _service.DeleteAsync(id, tenantId, userId);
        
        if (!result.Success)
            return NotFound(result);
        
        return Ok(result);
    }
}
```

---

## 🔌 Configuração de Injeção de Dependência (IoC)

### 📁 Estrutura
```
IbnelveApi.IoC/
├── DependencyInjection.cs  # Configurações principais
└── JwtTokenService.cs      # Serviço JWT
```

### ⚙️ Configuração Principal

```csharp
namespace IbnelveApi.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Identity
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // JWT
        var jwtSettings = configuration.GetSection("JwtSettings");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
                };
            });

        // Services
        services.AddScoped<IMembroService, MembroService>();
        services.AddScoped<ITarefaService, TarefaService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Repositories
        services.AddScoped<IMembroRepository, MembroRepository>();
        services.AddScoped<ITarefaRepository, TarefaRepository>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<CreateMembroDtoValidator>();

        // JWT Service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
```

---

## 🧪 Padrões de Testes

### 📁 Estrutura
```
IbnelveApi.Tests/
├── Unit/              # Testes unitários
│   ├── Services/      # Testes de serviços
│   ├── Validators/    # Testes de validadores
│   └── Mappings/      # Testes de mapeamentos
├── Integration/       # Testes de integração
└── Fixtures/         # Dados de teste
```

### 🧪 Exemplo de Teste Unitário

```csharp
namespace IbnelveApi.Tests.Unit.Services;

public class MembroServiceTests
{
    private readonly Mock<IMembroRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateMembroDto>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateMembroDto>> _updateValidatorMock;
    private readonly MembroService _service;

    public MembroServiceTests()
    {
        _repositoryMock = new Mock<IMembroRepository>();
        _createValidatorMock = new Mock<IValidator<CreateMembroDto>>();
        _updateValidatorMock = new Mock<IValidator<UpdateMembroDto>>();
        
        _service = new MembroService(
            _repositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsSuccessResult()
    {
        // Arrange
        var dto = new CreateMembroDto
        {
            Nome = "João Silva",
            CPF = "12345678901",
            Telefone = "11999999999",
            Endereco = new CreateEnderecoDto
            {
                Rua = "Rua Teste, 123",
                CEP = "12345678",
                Bairro = "Centro",
                Cidade = "São Paulo",
                UF = "SP"
            }
        };

        var validationResult = new ValidationResult();
        _createValidatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(validationResult);

        _repositoryMock.Setup(r => r.GetByCpfAsync(dto.CPF, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((Membro?)null);

        var createdEntity = MembroMapping.ToEntity(dto, "tenant1", "user1");
        createdEntity.Id = 1;
        
        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Membro>()))
            .ReturnsAsync(createdEntity);

        // Act
        var result = await _service.CreateAsync(dto, "tenant1", "user1");

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("João Silva", result.Data.Nome);
        Assert.Equal("Membro criado com sucesso", result.Message);
    }
}
```

---

## 📋 Convenções e Boas Práticas

### 🏷️ Nomenclatura

#### Classes e Interfaces
- **Entidades**: PascalCase (ex: `Membro`, `Tarefa`)
- **DTOs**: PascalCase + sufixo `Dto` (ex: `MembroDto`, `CreateMembroDto`)
- **Services**: PascalCase + sufixo `Service` (ex: `MembroService`)
- **Interfaces**: PascalCase com prefixo `I` (ex: `IMembroService`)
- **Validators**: PascalCase + sufixo `Validator` (ex: `CreateMembroDtoValidator`)

#### Métodos
- **CRUD**: `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`
- **Busca**: `GetByXAsync`, `FindByXAsync`, `SearchByXAsync`
- **Validação**: `BeValidX`, `IsValidX`

#### Propriedades
- **Entidades**: PascalCase (ex: `Nome`, `TenantId`)
- **DTOs**: PascalCase (ex: `Nome`, `DataVencimento`)

### 🔧 Configurações

#### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SERVER;Database=IbnelveApiDB01;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "sua-chave-super-secreta-aqui-com-mais-de-32-caracteres",
    "Issuer": "IbnelveApi",
    "Audience": "IbnelveApiUsers",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 🎯 Validações Comuns

#### CPF
```csharp
RuleFor(x => x.CPF)
    .NotEmpty().WithMessage("CPF é obrigatório")
    .Must(cpf => !string.IsNullOrEmpty(cpf?.RemoveSpecialCharacters()))
        .WithMessage("CPF é obrigatório")
    .Must(cpf => cpf.RemoveSpecialCharacters().Length == 11)
        .WithMessage("CPF deve conter exatamente 11 dígitos")
    .Must(cpf => ValidateCPF.BeValidCPF(cpf.RemoveSpecialCharacters()))
        .WithMessage("CPF inválido");
```

#### Telefone
```csharp
RuleFor(x => x.Telefone)
    .NotEmpty().WithMessage("Telefone é obrigatório")
    .Must(telefone => telefone.RemoveSpecialCharacters().Length >= 10)
        .WithMessage("Telefone deve ter pelo menos 10 dígitos")
    .Must(telefone => telefone.RemoveSpecialCharacters().Length <= 15)
        .WithMessage("Telefone deve ter no máximo 15 dígitos")
    .Matches(@"^[\d\s\(\)\-\+]+$").WithMessage("Telefone contém caracteres inválidos");
```

#### Endereço
```csharp
RuleFor(x => x.Endereco)
    .NotNull().WithMessage("Endereço é obrigatório")
    .SetValidator(new CreateEnderecoDtoValidator());

// EnderecoDtoValidator
RuleFor(x => x.CEP)
    .NotEmpty().WithMessage("CEP é obrigatório")
    .Must(cep => cep.RemoveSpecialCharacters().Length == 8)
        .WithMessage("CEP deve conter exatamente 8 dígitos");

RuleFor(x => x.UF)
    .NotEmpty().WithMessage("UF é obrigatória")
    .Length(2).WithMessage("UF deve ter exatamente 2 caracteres")
    .Matches(@"^[A-Z]{2}$").WithMessage("UF deve conter apenas letras maiúsculas");
```

### 🔍 Métodos de Extensão Úteis

```csharp
namespace IbnelveApi.Application.Extensions;

public static class StringExtensions
{
    public static string RemoveSpecialCharacters(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;
            
        return Regex.Replace(input, @"[^\d]", "");
    }

    public static string FormatCPF(this string cpf)
    {
        if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
            return cpf;
            
        return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
    }

    public static string FormatTelefone(this string telefone)
    {
        if (string.IsNullOrEmpty(telefone))
            return telefone;
            
        var digits = telefone.RemoveSpecialCharacters();
        
        return digits.Length switch
        {
            10 => $"({digits.Substring(0, 2)}) {digits.Substring(2, 4)}-{digits.Substring(6, 4)}",
            11 => $"({digits.Substring(0, 2)}) {digits.Substring(2, 5)}-{digits.Substring(7, 4)}",
            _ => telefone
        };
    }
}
```

---

## 🚀 Comandos Úteis para Desenvolvimento

### 🔨 Compilação e Execução
```bash
# Restaurar dependências
dotnet restore

# Compilar projeto
dotnet build

# Executar em modo de desenvolvimento
cd IbnelveApi.Api
dotnet run

# Executar testes
dotnet test

# Publicar para produção
dotnet publish -c Release
```

### 🗄️ Entity Framework
```bash
# Adicionar nova migração
dotnet ef migrations add NomeDaMigracao -p IbnelveApi.Infrastructure -s IbnelveApi.Api

# Atualizar banco de dados
dotnet ef database update -p IbnelveApi.Infrastructure -s IbnelveApi.Api

# Reverter migração
dotnet ef database update MigracaoAnterior -p IbnelveApi.Infrastructure -s IbnelveApi.Api

# Gerar script SQL
dotnet ef migrations script -p IbnelveApi.Infrastructure -s IbnelveApi.Api
```

### 📦 NuGet Packages Utilizados
```xml
<!-- API Layer -->
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.1" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.4" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.7" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="9.0.3" />

<!-- Infrastructure Layer -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.7" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.4" />

<!-- Application Layer -->
<PackageReference Include="FluentValidation" Version="11.9.2" />

<!-- Tests -->
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
<PackageReference Include="Moq" Version="4.20.70" />
```

---

## 🎯 Endpoints da API

### 🔐 Autenticação (`/api/auth`)
- `POST /register` - Registrar novo usuário
- `POST /login` - Realizar login e obter token JWT

### 👤 Membro (`/api/membro`) - Requer Autenticação
- `GET /` - Listar todos os membros
- `GET /{id}` - Buscar membro por ID
- `GET /cpf/{cpf}` - Buscar membro por CPF
- `GET /search?nome={nome}` - Buscar membros por nome
- `POST /` - Criar novo membro
- `PUT /{id}` - Atualizar membro
- `DELETE /{id}` - Excluir membro (soft delete)

### 📋 Tarefa (`/api/tarefa`) - Requer Autenticação
- `GET /` - Listar todas as tarefas
- `GET /{id}` - Buscar tarefa por ID
- `GET /vencidas` - Listar tarefas vencidas
- `GET /status/{status}` - Filtrar por status
- `POST /` - Criar nova tarefa
- `PUT /{id}` - Atualizar tarefa
- `DELETE /{id}` - Excluir tarefa (soft delete)

### 🏷️ Categoria de Tarefa (`/api/categoriatarefa`) - Requer Autenticação
- `GET /` - Listar todas as categorias
- `GET /{id}` - Buscar categoria por ID
- `POST /` - Criar nova categoria
- `PUT /{id}` - Atualizar categoria
- `DELETE /{id}` - Excluir categoria (soft delete)

---

## 🔍 Exemplos de Uso com GitHub Copilot

### 🎯 Prompts Eficazes

#### Para criar nova entidade:
```
// Criar entidade Projeto seguindo o padrão UserOwnedEntity do IbnelveApi
// com propriedades: Nome, Descricao, DataInicio, DataFim, Status (enum), Orcamento (decimal)
// incluir relacionamento com Membro e Tarefa
```

#### Para criar DTO:
```
// Criar CreateProjetoDto seguindo padrão do IbnelveApi
// incluir todas as propriedades necessárias para criação
```

#### Para criar validador:
```
// Criar CreateProjetoDtoValidator seguindo padrão FluentValidation do IbnelveApi
// validar Nome (obrigatório, 2-200 chars), DataInicio < DataFim, Orcamento > 0
```

#### Para criar serviço:
```
// Criar ProjetoService seguindo padrão IbnelveApi
// implementar IProjetoService com métodos CRUD padrão
// incluir validações e mapeamentos
```

#### Para criar controller:
```
// Criar ProjetoController seguindo padrão IbnelveApi
// incluir todos os endpoints CRUD com documentação Swagger
// usar ICurrentUserService para tenantId/userId
```

### 🔧 Snippets Úteis

#### Método de serviço completo:
```csharp
public async Task<ApiResponse<{Entity}Dto>> Create{Entity}Async(Create{Entity}Dto dto, string tenantId, string userId)
{
    // Seguir padrão: validação -> verificação duplicação -> mapeamento -> criação -> resposta
}
```

#### Configuração EF Core:
```csharp
public void Configure(EntityTypeBuilder<{Entity}> builder)
{
    // Seguir padrão: chave -> propriedades -> índices -> query filter -> relacionamentos
}
```

---

## 📚 Documentação Adicional

- **README.md**: Documentação principal do projeto
- **README_TAREFAS.md**: Documentação específica do sistema de tarefas
- **API_ENDPOINTS.md**: Lista detalhada de todos os endpoints
- **todo.md**: Lista de tarefas e marcos do projeto

---

## 🏁 Conclusão

Este arquivo MCP fornece um contexto abrangente do projeto IbnelveApi para acelerar o desenvolvimento com GitHub Copilot. Ele inclui:

- ✅ Arquitetura e padrões estabelecidos
- ✅ Exemplos de código para todas as camadas
- ✅ Convenções de nomenclatura e organização
- ✅ Validações e boas práticas
- ✅ Comandos úteis para desenvolvimento
- ✅ Prompts eficazes para o Copilot

Com este contexto, o GitHub Copilot será capaz de gerar código que segue fielmente os padrões arquiteturais e convenções estabelecidas no projeto IbnelveApi.