# IbnelveApi - Contexto para Assistente Copilot

## ??? Arquitetura e Organização

- Projeto .NET 9, padrão Clean Architecture, multi-camadas:
  - **IbnelveApi.Api**: Controllers, Program.cs, configuração inicial.
  - **IbnelveApi.Application**: DTOs, Services, Interfaces, Validators, Responses.
  - **IbnelveApi.Domain**: Entidades, Value Objects, Interfaces de domínio.
  - **IbnelveApi.Infrastructure**: EF Core, Repositórios, DataSeeder.
  - **IbnelveApi.IoC**: Injeção de dependência, JWT Token Service.
  - **IbnelveApi.Tests**: Testes unitários.

## ?? Tecnologias

- .NET 9, ASP.NET Core, Entity Framework Core, SQL Server.
- Autenticação: ASP.NET Identity + JWT Bearer.
- Validação: FluentValidation.
- Documentação: Swagger/OpenAPI.

## ?? Multi-tenancy

- Todas as entidades principais (Pessoa, Tarefa, Membro) são isoladas por `tenantId`.
- Filtros globais por tenant em todos os serviços e repositórios.
- Usuários e dados de seed separados por tenant.

## ?? Pessoa

- CRUD completo, soft delete, busca por ID, CPF, nome.
- Validações robustas: CPF (formato, dígitos, não repetido), telefone, endereço (CEP, UF, limites).
- Endpoints exigem JWT Bearer.
- Filtros: incluir/excluir deletados, override de tenantId para admins.

## ??? Tarefas

- CRUD completo, soft delete, multi-tenancy.
- Status: Pendente, Em Andamento, Concluída, Cancelada.
- Prioridade: Baixa, Média, Alta, Crítica (crítica exige categoria).
- Filtros avançados: status, prioridade, categoria, vencimento, busca por título/descrição, ordenação.
- Validações: título (3-200), descrição (5-1000), prioridade válida, data de vencimento não retroativa, categoria opcional (max 100).

## ?? Membro

- CRUD completo, soft delete, multi-tenancy.
- Busca por ID, CPF, nome, filtros avançados (cidade, UF, nome, CPF, ordenação).
- Validações: CPF único por tenant, formato, telefone, endereço.
- Todos métodos do serviço usam tenantId para garantir isolamento.

## ?? Convenções e Padrões

- Responses padronizadas via `ApiResponse<T>`.
- DTOs para entrada/saída, mapeamentos manuais.
- Validações centralizadas em Validators (FluentValidation).
- Controllers organizados por entidade.
- Soft delete implementado em entidades e repositórios.
- Injeção de dependência via IoC.

## ?? Autenticação e Usuários

- Registro e login via endpoints `/api/auth/register` e `/api/auth/login`.
- JWT configurado em `appsettings.json` (SecretKey, Issuer, Audience, Expiration).
- Usuários de seed: admin1@ibnelveapi.com (tenant1), admin2@ibnelveapi.com (tenant2).

## ?? Endpoints Principais

- **Pessoa**: `/api/pessoa`, `/api/pessoa/{id}`, `/api/pessoa/cpf/{cpf}`, `/api/pessoa/search?nome={nome}`
- **Tarefa**: `/api/tarefa`, `/api/tarefa/{id}`, `/api/tarefa/filtros`, `/api/tarefa/search?searchTerm={termo}`, `/api/tarefa/status/{status}`, `/api/tarefa/vencidas`, `/api/tarefa/concluidas`
- **Membro**: `/api/membro`, `/api/membro/{id}`, `/api/membro/cpf/{cpf}`, `/api/membro/search?nome={nome}` (padrão similar à Pessoa)
- Todos exigem header `Authorization: Bearer {token}`.

## ?? Testes e Documentação

- Swagger UI disponível em `http://localhost:5000`.
- Dados de seed automáticos para testes.
- Testes unitários scaffolding em `IbnelveApi.Tests`.

## ?? Convenções de Código

- C# 13, .NET 9.
- Naming conventions: PascalCase para classes/métodos, camelCase para parâmetros/variáveis.
- Separação clara de responsabilidades por camada.
- Mapeamentos manuais para DTOs.
- Tratamento de erros consistente via ApiResponse.

## ?? Pontos de Atenção

- Sempre filtrar por tenantId em queries e comandos.
- Validar CPF, telefone e endereço conforme regras.
- Soft delete: nunca remover fisicamente, sempre marcar como excluído.
- Prioridade Crítica em tarefas exige categoria preenchida.
- Endpoints de busca e filtros devem respeitar multi-tenancy e soft delete.

## ?? Estrutura de Pastas
# IbnelveApi - Contexto para Assistente Copilot

## ??? Arquitetura e Organização

- Projeto .NET 9, padrão Clean Architecture, multi-camadas:
  - **IbnelveApi.Api**: Controllers, Program.cs, configuração inicial.
  - **IbnelveApi.Application**: DTOs, Services, Interfaces, Validators, Responses.
  - **IbnelveApi.Domain**: Entidades, Value Objects, Interfaces de domínio.
  - **IbnelveApi.Infrastructure**: EF Core, Repositórios, DataSeeder.
  - **IbnelveApi.IoC**: Injeção de dependência, JWT Token Service.
  - **IbnelveApi.Tests**: Testes unitários.

## ?? Tecnologias

- .NET 9, ASP.NET Core, Entity Framework Core, SQL Server.
- Autenticação: ASP.NET Identity + JWT Bearer.
- Validação: FluentValidation.
- Documentação: Swagger/OpenAPI.

## ?? Multi-tenancy

- Todas as entidades principais (Pessoa, Tarefa, Membro) são isoladas por `tenantId`.
- Filtros globais por tenant em todos os serviços e repositórios.
- Usuários e dados de seed separados por tenant.

## ?? Pessoa

- CRUD completo, soft delete, busca por ID, CPF, nome.
- Validações robustas: CPF (formato, dígitos, não repetido), telefone, endereço (CEP, UF, limites).
- Endpoints exigem JWT Bearer.
- Filtros: incluir/excluir deletados, override de tenantId para admins.

## ??? Tarefas

- CRUD completo, soft delete, multi-tenancy.
- Status: Pendente, Em Andamento, Concluída, Cancelada.
- Prioridade: Baixa, Média, Alta, Crítica (crítica exige categoria).
- Filtros avançados: status, prioridade, categoria, vencimento, busca por título/descrição, ordenação.
- Validações: título (3-200), descrição (5-1000), prioridade válida, data de vencimento não retroativa, categoria opcional (max 100).

## ?? Membro

- CRUD completo, soft delete, multi-tenancy.
- Busca por ID, CPF, nome, filtros avançados (cidade, UF, nome, CPF, ordenação).
- Validações: CPF único por tenant, formato, telefone, endereço.
- Todos métodos do serviço usam tenantId para garantir isolamento.

## ?? Convenções e Padrões

- Responses padronizadas via `ApiResponse<T>`.
- DTOs para entrada/saída, mapeamentos manuais.
- Validações centralizadas em Validators (FluentValidation).
- Controllers organizados por entidade.
- Soft delete implementado em entidades e repositórios.
- Injeção de dependência via IoC.

## ?? Autenticação e Usuários

- Registro e login via endpoints `/api/auth/register` e `/api/auth/login`.
- JWT configurado em `appsettings.json` (SecretKey, Issuer, Audience, Expiration).
- Usuários de seed: admin1@ibnelveapi.com (tenant1), admin2@ibnelveapi.com (tenant2).

## ?? Endpoints Principais

- **Pessoa**: `/api/pessoa`, `/api/pessoa/{id}`, `/api/pessoa/cpf/{cpf}`, `/api/pessoa/search?nome={nome}`
- **Tarefa**: `/api/tarefa`, `/api/tarefa/{id}`, `/api/tarefa/filtros`, `/api/tarefa/search?searchTerm={termo}`, `/api/tarefa/status/{status}`, `/api/tarefa/vencidas`, `/api/tarefa/concluidas`
- **Membro**: `/api/membro`, `/api/membro/{id}`, `/api/membro/cpf/{cpf}`, `/api/membro/search?nome={nome}` (padrão similar à Pessoa)
- Todos exigem header `Authorization: Bearer {token}`.

## ?? Testes e Documentação

- Swagger UI disponível em `http://localhost:5000`.
- Dados de seed automáticos para testes.
- Testes unitários scaffolding em `IbnelveApi.Tests`.

## ?? Convenções de Código

- C# 13, .NET 9.
- Naming conventions: PascalCase para classes/métodos, camelCase para parâmetros/variáveis.
- Separação clara de responsabilidades por camada.
- Mapeamentos manuais para DTOs.
- Tratamento de erros consistente via ApiResponse.

## ?? Pontos de Atenção

- Sempre filtrar por tenantId em queries e comandos.
- Validar CPF, telefone e endereço conforme regras.
- Soft delete: nunca remover fisicamente, sempre marcar como excluído.
- Prioridade Crítica em tarefas exige categoria preenchida.
- Endpoints de busca e filtros devem respeitar multi-tenancy e soft delete.

## ?? Estrutura de Pastas
