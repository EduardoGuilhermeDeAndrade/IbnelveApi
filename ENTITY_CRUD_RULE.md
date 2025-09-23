# Rule: Cria��o de Entidade + CRUD Completo (IbnelveApi)

## 1. Entidade
- Crie a entidade em IbnelveApi.Domain, herdando de GlobalEntity, TenantEntity ou UserOwnedEntity conforme necess�rio.
- Propriedades: use PascalCase, tipos adequados, e inclua TenantId/UserId se multitenant/user-owned.

## 2. DTOs
- Crie em IbnelveApi.Application.DTOs:
  - CreateXDto (entrada para cria��o)
  - UpdateXDto (entrada para atualiza��o)
  - XDto (leitura/retorno)

## 3. Validator
- Crie em IbnelveApi.Application.Validators:
  - CreateXDtoValidator : AbstractValidator<CreateXDto>
  - UpdateXDtoValidator : AbstractValidator<UpdateXDto>
- Implemente regras de neg�cio e valida��o obrigat�ria.

## 4. Mapping
- Crie/atualize mapeamento manual ou configure AutoMapper em IbnelveApi.Application.Mappings.

## 5. Reposit�rio
- Interface em IbnelveApi.Domain.Repositories (ex: IXRepository).
- Implementa��o em IbnelveApi.Infrastructure.Repositories.
- M�todos: GetAllAsync, GetByIdAsync, CreateAsync, UpdateAsync, DeleteAsync (soft delete).
- Filtre por TenantId/UserId e IsDeleted.

## 6. EF Core Configuration
- Crie configura��o em IbnelveApi.Infrastructure.Persistence.Configurations usando IEntityTypeConfiguration<T>.
- Configure HasQueryFilter para IsDeleted.
- Use Fluent API, nunca Data Annotations.

## 7. Service
- Crie servi�o em IbnelveApi.Application.Services (ex: XService).
- M�todos: CRUD, valida��o, orquestra��o, retorno via ApiResponse<T>.

## 8. Controller
- Crie controller em IbnelveApi.Api.Controllers.
- Use [ApiController], [Route("api/[controller]")], [Authorize] se necess�rio.
- Recupere tenantId/userId via ICurrentUserService.
- Valide DTOs, delegue para service, retorne ApiResponse<T>.

## 9. Testes
- Crie testes unit�rios (xUnit + Moq) para Service, Validator e Mapping em IbnelveApi.Tests.

---

Siga esta regra para garantir que cada nova entidade tenha um CRUD completo, seguro e alinhado ao padr�o do projeto.
