# Model Context Protocol (MCP) — IbnelveApi

## 1. Propósito
Fornecer contexto conciso ao Copilot para gerar código alinhado com as convenções e a arquitetura do projeto **IbnelveApi** (API RESTful, .NET 9, Clean Architecture, multi-tenant).

---

## 2. Linguagem e runtime
- **Linguagem:** C# (C# 12)
- **Runtime:** .NET 9 / ASP.NET Core
- Sugestões devem usar sintaxe moderna compatível com essas versões.

---

## 3. Arquitetura e organização de pastas
- **Padrão:** Clean Architecture (separar responsabilidades por camadas).
- Estrutura esperada:
  - `IbnelveApi.Api/` — apresentação (controllers, middlewares)
  - `IbnelveApi.Application/` — DTOs, interfaces, services, validators, mappings
  - `IbnelveApi.Domain/` — entidades, enums, value objects, interfaces de domínio
  - `IbnelveApi.Infrastructure/` — DbContext, configurações EF, repositórios
  - `IbnelveApi.IoC/` — injeção de dependência
  - `IbnelveApi.Tests/` — testes unitários e integração

Ao sugerir código, manter dependências unidirecionais (Apresentação → Aplicação → Infraestrutura → Domínio). Não colocar lógica de negócio na camada Api.

---

## 4. Nomenclatura e estilo de código (curto)
- **Classes/DTOs/Enums/Interfaces:** PascalCase (`Membro`, `CreateMembroDto`, `IMembroService`).
- **Métodos assíncronos:** sufixo `Async` (`GetAllAsync`).
- **Variáveis locais:** camelCase.
- **Nomes de validators:** `CreateXDtoValidator` / `UpdateXDtoValidator`.
- **Comentários:** preferir código autoexplicativo; XML comments apenas em APIs públicas e controladores.

---

## 5. Padrões e práticas obrigatórios
- **SOLID** sempre.
- **Repository Pattern** para acesso a dados (interfaces no domínio, implementações na infrastructure).
- **Service Layer** (Application) para orquestração e regras de aplicação.
- **DTOs** para entrada/saída; não expor entidades do domínio diretamente.
- **Soft delete**: usar `IsDeleted` + `HasQueryFilter` no EF Core.
- **Multitenancy**: entidades user-owned ou tenant-owned devem conter `TenantId` e, quando aplicável, `UserId`.

---

## 6. Principais bibliotecas (priorizar nas sugestões)
- **Entity Framework Core** (configurar via Fluent API)
- **FluentValidation** (validadores separados)
- **Swashbuckle / OpenAPI** (documentação)
- **ASP.NET Identity** (quando necessário)
- **xUnit + Moq** (testes unitários)

Observação: evitar Data Annotations para configuração do EF; sempre preferir Fluent API.

---

## 7. Entidades e herança padrão
- **GlobalEntity** — Id, IsDeleted, CreatedAt, UpdatedAt, excluir logicamente via `ExcluirLogicamente()`.
- **TenantEntity : GlobalEntity** — `TenantId` (string).
- **UserOwnedEntity : TenantEntity** — `UserId` (string).

Ao criar novas entidades, seguir essa hierarquia quando aplicável.

---

## 8. DTOs, Mappings e Validators
- **DTOs:** `CreateXDto`, `UpdateXDto`, `XDto` (read).
- **Mappings:** métodos manuais estáticos em `Application.Mappings` ou AutoMapper configurado em Application.
- **Validators:** regras em classes `AbstractValidator<T>`; sempre validar DTOs antes de persitir.

---

## 9. Repositórios e consultas
- Interfaces de repositório no domínio (ex.: `IUserOwnedRepository<T>`).
- Implementações na Infrastructure usando `DbContext` e `DbSet<T>`.
- Métodos esperados: `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync` (soft delete).
- Aplicar filtros por `TenantId`/`UserId` e respeitar `includeDeleted` quando for parâmetro.

---

## 10. Configuração EF Core (regras rápidas)
- Usar `IEntityTypeConfiguration<T>` para configurar tabelas.
- Definir `HasQueryFilter(m => !m.IsDeleted)` para soft delete.
- Criar índices compostos quando necessário (ex.: TenantId + UserId + CPF) com `HasFilter("[IsDeleted] = 0")` para SQL Server.
- Value Objects devem ser `OwnsOne`.

---

## 11. Controllers / Api
- Controllers devem ser finos: validar modelo (ModelState), recuperar `tenantId`/`userId` via `ICurrentUserService`, delegar para Application services e retornar `ApiResponse<T>`.
- Atributos: `[ApiController]`, `[Route("api/[controller]")]`, `[Authorize]` quando necessário.

---

## 12. Testes
- **Unit tests:** xUnit + Moq; cobrir Services, Validators e Mappings.
- **Integration tests:** usar banco em memória ou dockerized SQL Server quando necessário.
- Nome de teste: `MethodName_Scenario_ExpectedResult`.

---

## 13. Segurança e segredos
- **NÃO** commit API keys, secrets ou connection strings. Preferir `IConfiguration` + environment variables.
- JWT configurado via `JwtSettings` no `appsettings.json` e lido pelo `JwtBearer`.

---

## 14. Git / PRs (curto)
- Mensagens no padrão `feat|fix|chore(scope): descrição` (Conventional Commits).
- Cada PR deve conter objetivo, mudanças principais e instruções de teste.

---

## 15. Exemplos rápidos de prompts para Copilot
- `// Criar entidade Projeto seguindo o padrão UserOwnedEntity ...` (especifique propriedades)
- `// Criar CreateProjetoDto e seu validator seguindo padrões.`
- `// Implementar ProjetoService com validação -> duplicidade -> criação -> retorno ApiResponse`

---

## 16. Como o Copilot deve priorizar
1. Seguir a arquitetura e convenções descritas acima.  
2. Priorizar Fluent API sobre Data Annotations para EF.  
3. Sugerir validações via FluentValidation.  
4. Evitar exposição de entidades nos endpoints (usar DTOs).  

---

## 17. Observações finais
- Mantenha este documento sucinto; atualize quando houver mudanças de arquitetura ou versões.
- Para casos ambíguos, priorizar segurança, testabilidade e separação de responsabilidades.

---

*Fim do MCP — IbnelveApi*

