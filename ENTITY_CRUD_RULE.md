# Rule: Criação de Entidade + CRUD Completo (IbnelveApi)

## 1. Entidade
- Crie a entidade em IbnelveApi.Domain, herdando de GlobalEntity, TenantEntity ou UserOwnedEntity conforme necessário.
- Propriedades: use PascalCase, tipos adequados, e inclua TenantId/UserId se multitenant/user-owned.

## 2. DTOs
- Crie em IbnelveApi.Application.DTOs:
  - CreateXDto (entrada para criação)
  - UpdateXDto (entrada para atualização)
  - XDto (leitura/retorno)

## 3. Validator
- Crie em IbnelveApi.Application.Validators:
  - CreateXDtoValidator : AbstractValidator<CreateXDto>
  - UpdateXDtoValidator : AbstractValidator<UpdateXDto>
- Implemente regras de negócio e validação obrigatória.

## 4. Mapping
- Crie/atualize mapeamento manual ou configure AutoMapper em IbnelveApi.Application.Mappings.

## 5. Repositório
- Interface em IbnelveApi.Domain.Repositories (ex: IXRepository).
- Implementação em IbnelveApi.Infrastructure.Repositories.
- Métodos: GetAllAsync, GetByIdAsync, CreateAsync, UpdateAsync, DeleteAsync (soft delete).
- Filtre por TenantId/UserId e IsDeleted.

## 6. EF Core Configuration
- Crie configuração em IbnelveApi.Infrastructure.Persistence.Configurations usando IEntityTypeConfiguration<T>.
- Configure HasQueryFilter para IsDeleted.
- Use Fluent API, nunca Data Annotations.

## 7. Service
- Crie serviço em IbnelveApi.Application.Services (ex: XService).
- Métodos: CRUD, validação, orquestração, retorno via ApiResponse<T>.

## 8. Controller
- Crie controller em IbnelveApi.Api.Controllers.
- Use [ApiController], [Route("api/[controller]")], [Authorize] se necessário.
- Recupere tenantId/userId via ICurrentUserService.
- Valide DTOs, delegue para service, retorne ApiResponse<T>.

## 9. Testes
- Crie testes unitários (xUnit + Moq) para Service, Validator e Mapping em IbnelveApi.Tests.

---

Siga esta regra para garantir que cada nova entidade tenha um CRUD completo, seguro e alinhado ao padrão do projeto.
