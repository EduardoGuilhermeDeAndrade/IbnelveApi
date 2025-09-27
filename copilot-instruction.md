# IbnelveApi - Contexto para Assistente Copilot

## ??? Arquitetura e Organiza��o

- Projeto .NET 9, padr�o Clean Architecture, multi-camadas:
  - **IbnelveApi.Api**: Controllers, Program.cs, configura��o inicial.
  - **IbnelveApi.Application**: DTOs, Services, Interfaces, Validators, Responses.
  - **IbnelveApi.Domain**: Entidades, Value Objects, Interfaces de dom�nio.
  - **IbnelveApi.Infrastructure**: EF Core, Reposit�rios, DataSeeder.
  - **IbnelveApi.IoC**: Inje��o de depend�ncia, JWT Token Service.
  - **IbnelveApi.Tests**: Testes unit�rios.

## ?? Tecnologias

- .NET 9, ASP.NET Core, Entity Framework Core, SQL Server.
- Autentica��o: ASP.NET Identity + JWT Bearer.
- Valida��o: FluentValidation.
- Documenta��o: Swagger/OpenAPI.

## ?? Multi-tenancy

- Todas as entidades principais (Pessoa, Tarefa, Membro) s�o isoladas por `tenantId`.
- Filtros globais por tenant em todos os servi�os e reposit�rios.
- Usu�rios e dados de seed separados por tenant.

## ?? Pessoa

- CRUD completo, soft delete, busca por ID, CPF, nome.
- Valida��es robustas: CPF (formato, d�gitos, n�o repetido), telefone, endere�o (CEP, UF, limites).
- Endpoints exigem JWT Bearer.
- Filtros: incluir/excluir deletados, override de tenantId para admins.

## ??? Tarefas

- CRUD completo, soft delete, multi-tenancy.
- Status: Pendente, Em Andamento, Conclu�da, Cancelada.
- Prioridade: Baixa, M�dia, Alta, Cr�tica (cr�tica exige categoria).
- Filtros avan�ados: status, prioridade, categoria, vencimento, busca por t�tulo/descri��o, ordena��o.
- Valida��es: t�tulo (3-200), descri��o (5-1000), prioridade v�lida, data de vencimento n�o retroativa, categoria opcional (max 100).

## ?? Membro

- CRUD completo, soft delete, multi-tenancy.
- Busca por ID, CPF, nome, filtros avan�ados (cidade, UF, nome, CPF, ordena��o).
- Valida��es: CPF �nico por tenant, formato, telefone, endere�o.
- Todos m�todos do servi�o usam tenantId para garantir isolamento.

## ?? Conven��es e Padr�es

- Responses padronizadas via `ApiResponse<T>`.
- DTOs para entrada/sa�da, mapeamentos manuais.
- Valida��es centralizadas em Validators (FluentValidation).
- Controllers organizados por entidade.
- Soft delete implementado em entidades e reposit�rios.
- Inje��o de depend�ncia via IoC.

## ?? Autentica��o e Usu�rios

- Registro e login via endpoints `/api/auth/register` e `/api/auth/login`.
- JWT configurado em `appsettings.json` (SecretKey, Issuer, Audience, Expiration).
- Usu�rios de seed: admin1@ibnelveapi.com (tenant1), admin2@ibnelveapi.com (tenant2).

## ?? Endpoints Principais

- **Pessoa**: `/api/pessoa`, `/api/pessoa/{id}`, `/api/pessoa/cpf/{cpf}`, `/api/pessoa/search?nome={nome}`
- **Tarefa**: `/api/tarefa`, `/api/tarefa/{id}`, `/api/tarefa/filtros`, `/api/tarefa/search?searchTerm={termo}`, `/api/tarefa/status/{status}`, `/api/tarefa/vencidas`, `/api/tarefa/concluidas`
- **Membro**: `/api/membro`, `/api/membro/{id}`, `/api/membro/cpf/{cpf}`, `/api/membro/search?nome={nome}` (padr�o similar � Pessoa)
- Todos exigem header `Authorization: Bearer {token}`.

## ?? Testes e Documenta��o

- Swagger UI dispon�vel em `http://localhost:5000`.
- Dados de seed autom�ticos para testes.
- Testes unit�rios scaffolding em `IbnelveApi.Tests`.

## ?? Conven��es de C�digo

- C# 13, .NET 9.
- Naming conventions: PascalCase para classes/m�todos, camelCase para par�metros/vari�veis.
- Separa��o clara de responsabilidades por camada.
- Mapeamentos manuais para DTOs.
- Tratamento de erros consistente via ApiResponse.

## ?? Pontos de Aten��o

- Sempre filtrar por tenantId em queries e comandos.
- Validar CPF, telefone e endere�o conforme regras.
- Soft delete: nunca remover fisicamente, sempre marcar como exclu�do.
- Prioridade Cr�tica em tarefas exige categoria preenchida.
- Endpoints de busca e filtros devem respeitar multi-tenancy e soft delete.

## ?? Estrutura de Pastas
# IbnelveApi - Contexto para Assistente Copilot

## ??? Arquitetura e Organiza��o

- Projeto .NET 9, padr�o Clean Architecture, multi-camadas:
  - **IbnelveApi.Api**: Controllers, Program.cs, configura��o inicial.
  - **IbnelveApi.Application**: DTOs, Services, Interfaces, Validators, Responses.
  - **IbnelveApi.Domain**: Entidades, Value Objects, Interfaces de dom�nio.
  - **IbnelveApi.Infrastructure**: EF Core, Reposit�rios, DataSeeder.
  - **IbnelveApi.IoC**: Inje��o de depend�ncia, JWT Token Service.
  - **IbnelveApi.Tests**: Testes unit�rios.

## ?? Tecnologias

- .NET 9, ASP.NET Core, Entity Framework Core, SQL Server.
- Autentica��o: ASP.NET Identity + JWT Bearer.
- Valida��o: FluentValidation.
- Documenta��o: Swagger/OpenAPI.

## ?? Multi-tenancy

- Todas as entidades principais (Pessoa, Tarefa, Membro) s�o isoladas por `tenantId`.
- Filtros globais por tenant em todos os servi�os e reposit�rios.
- Usu�rios e dados de seed separados por tenant.

## ?? Pessoa

- CRUD completo, soft delete, busca por ID, CPF, nome.
- Valida��es robustas: CPF (formato, d�gitos, n�o repetido), telefone, endere�o (CEP, UF, limites).
- Endpoints exigem JWT Bearer.
- Filtros: incluir/excluir deletados, override de tenantId para admins.

## ??? Tarefas

- CRUD completo, soft delete, multi-tenancy.
- Status: Pendente, Em Andamento, Conclu�da, Cancelada.
- Prioridade: Baixa, M�dia, Alta, Cr�tica (cr�tica exige categoria).
- Filtros avan�ados: status, prioridade, categoria, vencimento, busca por t�tulo/descri��o, ordena��o.
- Valida��es: t�tulo (3-200), descri��o (5-1000), prioridade v�lida, data de vencimento n�o retroativa, categoria opcional (max 100).

## ?? Membro

- CRUD completo, soft delete, multi-tenancy.
- Busca por ID, CPF, nome, filtros avan�ados (cidade, UF, nome, CPF, ordena��o).
- Valida��es: CPF �nico por tenant, formato, telefone, endere�o.
- Todos m�todos do servi�o usam tenantId para garantir isolamento.

## ?? Conven��es e Padr�es

- Responses padronizadas via `ApiResponse<T>`.
- DTOs para entrada/sa�da, mapeamentos manuais.
- Valida��es centralizadas em Validators (FluentValidation).
- Controllers organizados por entidade.
- Soft delete implementado em entidades e reposit�rios.
- Inje��o de depend�ncia via IoC.

## ?? Autentica��o e Usu�rios

- Registro e login via endpoints `/api/auth/register` e `/api/auth/login`.
- JWT configurado em `appsettings.json` (SecretKey, Issuer, Audience, Expiration).
- Usu�rios de seed: admin1@ibnelveapi.com (tenant1), admin2@ibnelveapi.com (tenant2).

## ?? Endpoints Principais

- **Pessoa**: `/api/pessoa`, `/api/pessoa/{id}`, `/api/pessoa/cpf/{cpf}`, `/api/pessoa/search?nome={nome}`
- **Tarefa**: `/api/tarefa`, `/api/tarefa/{id}`, `/api/tarefa/filtros`, `/api/tarefa/search?searchTerm={termo}`, `/api/tarefa/status/{status}`, `/api/tarefa/vencidas`, `/api/tarefa/concluidas`
- **Membro**: `/api/membro`, `/api/membro/{id}`, `/api/membro/cpf/{cpf}`, `/api/membro/search?nome={nome}` (padr�o similar � Pessoa)
- Todos exigem header `Authorization: Bearer {token}`.

## ?? Testes e Documenta��o

- Swagger UI dispon�vel em `http://localhost:5000`.
- Dados de seed autom�ticos para testes.
- Testes unit�rios scaffolding em `IbnelveApi.Tests`.

## ?? Conven��es de C�digo

- C# 13, .NET 9.
- Naming conventions: PascalCase para classes/m�todos, camelCase para par�metros/vari�veis.
- Separa��o clara de responsabilidades por camada.
- Mapeamentos manuais para DTOs.
- Tratamento de erros consistente via ApiResponse.

## ?? Pontos de Aten��o

- Sempre filtrar por tenantId em queries e comandos.
- Validar CPF, telefone e endere�o conforme regras.
- Soft delete: nunca remover fisicamente, sempre marcar como exclu�do.
- Prioridade Cr�tica em tarefas exige categoria preenchida.
- Endpoints de busca e filtros devem respeitar multi-tenancy e soft delete.

## ?? Estrutura de Pastas
