# Copilot Instruction — IbnelveApi

Este documento fornece **contexto e diretrizes obrigatórias** para que o GitHub Copilot gere código alinhado à arquitetura, convenções e práticas do projeto **IbnelveApi** (API RESTful, .NET 9, Clean Architecture, multi-tenant).

---

## 1. Linguagem e Runtime

* **Linguagem:** C# (C# 12/13)
* **Runtime:** .NET 9 / ASP.NET Core
* Gerar código sempre compatível com estas versões.

---

## 2. Arquitetura e Organização de Pastas

* **Padrão:** Clean Architecture
* **Camadas:**

  * `IbnelveApi.Api` — Controllers, Middlewares, Program.cs
  * `IbnelveApi.Application` — DTOs, Services, Interfaces, Validators, Mappings, Responses
  * `IbnelveApi.Domain` — Entidades, Enums, Value Objects, Interfaces de domínio
  * `IbnelveApi.Infrastructure` — EF Core (DbContext, Repositórios, Configurações)
  * `IbnelveApi.IoC` — Injeção de dependência, Configuração de JWT

**Regra:** Fluxo unidirecional (Api → Application → Infrastructure → Domain).
**Proibido:** lógica de negócio em Api.

---

## 3. Padrões de Código e Nomenclatura

* **Classes/DTOs/Enums/Interfaces:** PascalCase
* **Variáveis locais:** camelCase
* **Métodos assíncronos:** sufixo `Async`
* **Validators:** `CreateXDtoValidator`, `UpdateXDtoValidator`
* **Comentários:** preferir código autoexplicativo; XML apenas em APIs públicas

---

## 4. Entidades e Herança

* `GlobalEntity` — Id (Guid), IsDeleted, CreatedAt, UpdatedAt
* `TenantEntity : GlobalEntity` — adiciona `TenantId`
* `UserOwnedEntity : TenantEntity` — adiciona `UserId`

**Regras:**

* Soft delete com `IsDeleted` + `HasQueryFilter`
* Sempre filtrar por `TenantId` (e `UserId` quando aplicável)

---

## 5. Multi-tenancy

* Todas as entidades relevantes devem conter `TenantId`.
* Serviços e repositórios aplicam filtro global de tenant.
* Admins podem sobrescrever o `TenantId` em casos específicos.
* Usuários e dados de seed separados por tenant.

---

## 6. DTOs, Mappings e Validators

* **DTOs:** `CreateXDto`, `UpdateXDto`, `XDto` (read)
* **Mappings:** AutoMapper configurado na Application
* **Validators:** sempre baseados em FluentValidation, não em DataAnnotations

---

## 7. Repositórios e Infraestrutura

* **Interfaces**: no domínio
* **Implementações**: na infraestrutura
* Métodos obrigatórios:

  * `GetAllAsync`
  * `GetByIdAsync`
  * `CreateAsync`
  * `UpdateAsync`
  * `DeleteAsync` (soft delete)

**Observação:** sempre aplicar filtro por tenant e soft delete.

---

## 8. Controllers / API

* **Responsabilidades:**

  * Validar modelo (ModelState + Validator)
  * Recuperar `TenantId` / `UserId` via `ICurrentUserService`
  * Delegar para serviços da Application
  * Retornar `ApiResponse<T>`

* **Atributos:**

  * `[ApiController]`, `[Route("api/[controller]")]`
  * `[Authorize]` onde necessário

---

## 9. Segurança

* Autenticação via JWT Bearer (config em `appsettings.json`)
* **Nunca** expor secrets no repositório (usar env vars)
* Configurações sensíveis em `IConfiguration`

---

## 10. Convenções de Projeto

* Responses padronizadas via `ApiResponse<T>`
* Nunca expor entidades diretamente em endpoints
* Mapeamentos manuais ou AutoMapper centralizado
* Soft delete aplicado em todas as entidades
* Testabilidade e separação de responsabilidades são prioridade
* **Domínio não deve ser poluído com pacotes externos**

  * Apenas C# puro e construções da linguagem
  * Nada de EF Core, FluentValidation, bibliotecas de terceiros

---

## 11. Exemplos de Prompts para Copilot

* `// Criar entidade Projeto herdando de UserOwnedEntity com propriedades Nome (string, obrigatório, máx 200) e Descricao (string, opcional, máx 500).`
* `// Criar CreateProjetoDto, UpdateProjetoDto e ProjetoDto + validadores correspondentes.`
* `// Implementar ProjetoService com validação de duplicidade, persistência e retorno em ApiResponse.`
* `// Configurar IEntityTypeConfiguration para Projeto com índices em TenantId + Nome.`

---

## 12. Prioridades do Copilot

1. Seguir arquitetura e convenções deste documento
2. Priorizar **Fluent API** em vez de Data Annotations
3. Usar **FluentValidation** em DTOs
4. Respeitar multi-tenancy em consultas e comandos
5. Retornar sempre `ApiResponse<T>` em APIs
6. **Manter domínio 100% limpo, sem dependência de pacotes externos**

---

*Fim do Copilot Instruction — IbnelveApi*
