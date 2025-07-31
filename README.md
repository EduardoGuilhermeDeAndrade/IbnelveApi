# IbnelveApi

API RESTful desenvolvida com .NET 9 utilizando Clean Architecture, com suporte a multi-tenancy, autenticação JWT, soft delete e CRUD completo para entidade Pessoa.

## 🏗️ Arquitetura

O projeto segue os princípios da Clean Architecture, organizado em camadas bem definidas:

- **IbnelveApi.Api**: Camada de apresentação (Controllers, Program.cs)
- **IbnelveApi.Application**: Camada de aplicação (DTOs, Services, Interfaces, Validações)
- **IbnelveApi.Domain**: Camada de domínio (Entidades, Value Objects, Interfaces de domínio)
- **IbnelveApi.Infrastructure**: Camada de infraestrutura (Entity Framework, Repositórios)
- **IbnelveApi.IoC**: Configuração de Injeção de Dependência
- **IbnelveApi.Tests**: Testes unitários (scaffolding)

## 🚀 Tecnologias Utilizadas

- **.NET 9**: Framework principal
- **ASP.NET Core**: Web API
- **Entity Framework Core**: ORM para acesso a dados
- **SQL Server**: Banco de dados
- **ASP.NET Identity**: Sistema de autenticação
- **JWT Bearer**: Autenticação baseada em tokens
- **FluentValidation**: Validação de dados
- **Swagger/OpenAPI**: Documentação da API
- **Clean Architecture**: Padrão arquitetural

## 🔧 Funcionalidades

### Autenticação e Autorização
- Sistema de login com JWT
- Registro de novos usuários
- Multi-tenancy baseado em TenantId
- Tokens com expiração configurável

### CRUD de Pessoa
- Criar nova pessoa
- Listar pessoas (com filtros)
- Buscar pessoa por ID
- Buscar pessoa por CPF
- Buscar pessoas por nome
- Atualizar dados da pessoa
- Exclusão lógica (soft delete)

### Recursos Avançados
- **Multi-tenancy**: Isolamento de dados por tenant
- **Soft Delete**: Exclusão lógica com possibilidade de recuperação
- **Validação robusta**: CPF, telefone, endereço
- **Filtros de query**: incluir/excluir registros deletados
- **Documentação Swagger**: Interface interativa para testes

## 📋 Pré-requisitos

- .NET 9 SDK
- SQL Server (ou SQL Server Express)
- Visual Studio 2022 ou VS Code (opcional)

## ⚙️ Configuração

### 1. Clone o repositório
```bash
git clone <repository-url>
cd IbnelveApi
```

### 2. Configure a string de conexão
Edite o arquivo `IbnelveApi.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=IbnelveApiDB01;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Restaure as dependências
```bash
dotnet restore
```

### 4. Compile o projeto
```bash
dotnet build
```

## 🏃‍♂️ Executando a Aplicação

### Desenvolvimento
```bash
cd IbnelveApi.Api
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:5000`
- Swagger UI: `http://localhost:5000` (página inicial)

### Produção
```bash
dotnet publish -c Release
```

## 📚 Documentação da API

### Autenticação

#### POST /api/auth/register
Registra um novo usuário no sistema.

**Request Body:**
```json
{
  "email": "usuario@exemplo.com",
  "password": "MinhaSenh@123",
  "tenantId": "tenant1"
}
```

#### POST /api/auth/login
Realiza login e retorna token JWT.

**Request Body:**
```json
{
  "email": "usuario@exemplo.com",
  "password": "MinhaSenh@123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login realizado com sucesso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "email": "usuario@exemplo.com",
    "tenantId": "tenant1",
    "expiresAt": "2024-01-02T10:30:00Z"
  }
}
```

### Pessoa (Requer Autenticação)

Todos os endpoints de Pessoa requerem o header de autorização:
```
Authorization: Bearer {seu-token-jwt}
```

#### GET /api/pessoa
Lista todas as pessoas do tenant.

**Query Parameters:**
- `includeDeleted` (bool): Incluir registros excluídos logicamente
- `tenantId` (string): Override do TenantId (para cenários admin)

#### GET /api/pessoa/{id}
Busca pessoa por ID.

#### GET /api/pessoa/cpf/{cpf}
Busca pessoa por CPF.

#### GET /api/pessoa/search?nome={nome}
Busca pessoas por nome (busca parcial).

#### POST /api/pessoa
Cria nova pessoa.

**Request Body:**
```json
{
  "nome": "João Silva Santos",
  "cpf": "12345678901",
  "telefone": "(11) 99999-1111",
  "endereco": {
    "rua": "Rua das Flores, 123",
    "cep": "01234567",
    "bairro": "Centro",
    "cidade": "São Paulo",
    "uf": "SP"
  }
}
```

#### PUT /api/pessoa/{id}
Atualiza dados da pessoa.

#### DELETE /api/pessoa/{id}
Exclui pessoa logicamente (soft delete).

## 🗄️ Banco de Dados

### Configuração Automática
A aplicação cria automaticamente o banco de dados e as tabelas na primeira execução.

### Dados de Seed
São criados automaticamente:

**Usuários Admin:**
- `admin1@ibnelveapi.com` / `Admin123!` (TenantId: tenant1)
- `admin2@ibnelveapi.com` / `Admin123!` (TenantId: tenant2)

**Pessoas de Exemplo:**
- João Silva Santos (tenant1)
- Maria Oliveira Costa (tenant2)

## 🔐 Configurações JWT

As configurações JWT estão no `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "IbnelveApi-SuperSecretKey-2024-MinimumLength32Characters!",
    "Issuer": "IbnelveApi",
    "Audience": "IbnelveApi-Users",
    "ExpirationHours": "24"
  }
}
```

## 🧪 Testando a API

### 1. Via Swagger UI
Acesse `http://localhost:5000` e use a interface interativa.

### 2. Via cURL

**Login:**
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin1@ibnelveapi.com",
    "password": "Admin123!"
  }'
```

**Listar Pessoas:**
```bash
curl -X GET "http://localhost:5000/api/pessoa" \
  -H "Authorization: Bearer {seu-token}"
```

**Criar Pessoa:**
```bash
curl -X POST "http://localhost:5000/api/pessoa" \
  -H "Authorization: Bearer {seu-token}" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Teste da Silva",
    "cpf": "11122233344",
    "telefone": "(11) 88888-8888",
    "endereco": {
      "rua": "Rua de Teste, 456",
      "cep": "12345678",
      "bairro": "Bairro Teste",
      "cidade": "São Paulo",
      "uf": "SP"
    }
  }'
```

## 🔍 Validações Implementadas

### CPF
- Formato: 11 dígitos numéricos
- Validação de dígitos verificadores
- Não permite CPFs com todos os dígitos iguais

### Telefone
- Máximo 20 caracteres
- Permite números, espaços, parênteses, hífens e sinal de +

### Endereço
- CEP: 8 dígitos numéricos
- UF: 2 letras maiúsculas
- Campos obrigatórios com limites de tamanho

## 🏗️ Estrutura do Projeto

```
IbnelveApi/
├── IbnelveApi.Api/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   └── PessoaController.cs
│   ├── Models/
│   │   └── AuthModels.cs
│   ├── Program.cs
│   └── appsettings.json
├── IbnelveApi.Application/
│   ├── Common/
│   │   └── ApiResponse.cs
│   ├── DTOs/
│   │   ├── EnderecoDto.cs
│   │   └── PessoaDto.cs
│   ├── Interfaces/
│   │   └── IPessoaService.cs
│   ├── Mappings/
│   │   ├── EnderecoMapping.cs
│   │   └── PessoaMapping.cs
│   ├── Services/
│   │   └── PessoaService.cs
│   └── Validators/
│       ├── CreatePessoaDtoValidator.cs
│       ├── EnderecoDtoValidator.cs
│       └── UpdatePessoaDtoValidator.cs
├── IbnelveApi.Domain/
│   ├── Entities/
│   │   ├── BaseEntity.cs
│   │   └── Pessoa.cs
│   ├── Interfaces/
│   │   ├── IPessoaRepository.cs
│   │   └── IRepository.cs
│   └── ValueObjects/
│       └── Endereco.cs
├── IbnelveApi.Infrastructure/
│   ├── Configurations/
│   │   └── PessoaConfiguration.cs
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── DataSeeder.cs
│   └── Repositories/
│       ├── PessoaRepository.cs
│       └── Repository.cs
├── IbnelveApi.IoC/
│   ├── DependencyInjection.cs
│   └── JwtTokenService.cs
└── IbnelveApi.Tests/
```

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## 👥 Autores

- **Equipe IbnelveApi** - Desenvolvimento inicial

## 🙏 Agradecimentos

- Comunidade .NET
- Documentação oficial do ASP.NET Core
- Padrões de Clean Architecture

