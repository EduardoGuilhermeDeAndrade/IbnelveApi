# IbnelveApi

Uma aplicação .NET 9 completa seguindo Clean Architecture com Multi-Tenancy, JWT Authentication e Entity Framework Core.

## Arquitetura

A aplicação segue os princípios da Clean Architecture com separação clara de responsabilidades:

- **IbnelveApi.Domain**: Entidades, interfaces e regras de negócio
- **IbnelveApi.Application**: Casos de uso, DTOs, validações e serviços
- **IbnelveApi.Infrastructure**: Implementação de repositórios, DbContext e serviços externos
- **IbnelveApi.IoC**: Configuração de injeção de dependências
- **IbnelveApi.Api**: Minimal API, endpoints e middlewares

## Tecnologias Utilizadas

- **.NET 9**: Framework principal
- **Minimal API**: Endpoints sem controllers
- **Entity Framework Core**: ORM com SQL Server
- **ASP.NET Core Identity**: Autenticação e autorização
- **JWT Bearer**: Tokens de autenticação
- **AutoMapper**: Mapeamento entre objetos
- **FluentValidation**: Validação de dados
- **Swagger/OpenAPI**: Documentação da API

## Funcionalidades

### Multi-Tenancy
- Banco de dados compartilhado com campo `TenantId`
- Filtro global automático por tenant
- Middleware para resolução do tenant via header `X-Tenant-Id`

### Exclusão Lógica
- Campo `IsDeleted` em todas as entidades
- Filtro global para ocultar registros excluídos
- Operações de exclusão mantêm dados no banco

### CRUD de Produtos
- Criar, listar, obter, atualizar e excluir produtos
- Validações com FluentValidation
- Respostas padronizadas com `ApiResponse<T>`

### Autenticação JWT
- Registro e login de usuários
- Tokens JWT com expiração configurável
- Proteção de endpoints com `[Authorize]`

## Como Executar

### Pré-requisitos
- .NET 9 SDK
- SQL Server (LocalDB ou instância completa)

### Configuração

1. **Clone o repositório** (se aplicável)

2. **Configure a connection string** no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=IbnelveApiDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

3. **Aplique as migrations**:
```bash
cd IbnelveApi.Infrastructure
dotnet ef database update --startup-project ../IbnelveApi.Api
```

4. **Execute a aplicação**:
```bash
cd IbnelveApi.Api
dotnet run
```

5. **Acesse a documentação Swagger**:
   - URL: `https://localhost:5001` ou `http://localhost:5000`
   - A documentação Swagger estará disponível na raiz

## Uso da API

### Autenticação

1. **Registrar usuário**:
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "password": "MinhaSenh@123"
}
```

2. **Fazer login**:
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "password": "MinhaSenh@123"
}
```

3. **Usar o token** nos headers das requisições:
```http
Authorization: Bearer {seu-token-jwt}
X-Tenant-Id: tenant1
```

### Produtos

Todos os endpoints de produtos requerem autenticação:

- `GET /api/produtos` - Listar produtos
- `GET /api/produtos/{id}` - Obter produto por ID
- `POST /api/produtos` - Criar produto
- `PUT /api/produtos/{id}` - Atualizar produto
- `DELETE /api/produtos/{id}` - Excluir produto (lógica)

### Multi-Tenancy

Para testar o Multi-Tenancy, use diferentes valores no header `X-Tenant-Id`:

```http
X-Tenant-Id: tenant1
X-Tenant-Id: tenant2
```

Cada tenant verá apenas seus próprios dados.

## Dados de Exemplo

A aplicação inclui dados de seed:
- Usuário admin: `admin@ibnelveapi.com` / `Admin@123`
- 2 produtos de exemplo no tenant1

## Estrutura do Projeto

```
IbnelveApi/
├── IbnelveApi.Domain/
│   ├── Common/
│   │   └── BaseEntity.cs
│   ├── Entities/
│   │   └── Produto.cs
│   └── Interfaces/
│       ├── IRepository.cs
│       ├── IProdutoRepository.cs
│       └── ITenantContext.cs
├── IbnelveApi.Application/
│   ├── Common/
│   │   └── ApiResponse.cs
│   ├── DTOs/
│   ├── Services/
│   ├── Validators/
│   └── Mappings/
├── IbnelveApi.Infrastructure/
│   ├── Data/
│   ├── Repositories/
│   ├── Identity/
│   └── Services/
├── IbnelveApi.IoC/
│   └── DependencyInjection.cs
└── IbnelveApi.Api/
    ├── Endpoints/
    ├── Middleware/
    └── Program.cs
```

## Comandos Úteis

```bash
# Compilar a solução
dotnet build

# Executar testes (se houver)
dotnet test

# Criar nova migration
dotnet ef migrations add NomeDaMigration --startup-project IbnelveApi.Api --project IbnelveApi.Infrastructure

# Aplicar migrations
dotnet ef database update --startup-project IbnelveApi.Api --project IbnelveApi.Infrastructure

# Remover última migration
dotnet ef migrations remove --startup-project IbnelveApi.Api --project IbnelveApi.Infrastructure
```

## Contribuição

Este projeto segue as melhores práticas de Clean Architecture e SOLID. Contribuições são bem-vindas!

