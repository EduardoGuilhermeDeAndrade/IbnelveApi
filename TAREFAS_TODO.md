# Sistema de Gestão de Tarefas - Planejamento

## 🎯 Objetivo
Implementar sistema completo de gestão de tarefas no projeto IbnelveApi existente, seguindo a arquitetura Clean Architecture já estabelecida.

## 📋 Funcionalidades a Implementar

### Core Features
- [x] Analisar estrutura atual do projeto
- [x] Criar entidade Tarefa com propriedades essenciais
- [x] Implementar enums para Status e Prioridade
- [x] Criar DTOs para operações CRUD
- [x] Implementar repositório de tarefas
- [x] Criar serviços de aplicação
- [x] Implementar validações FluentValidation
- [x] Criar controller com endpoints REST
- [x] Configurar injeção de dependências
- [x] Criar seed de dados de exemplo
- [x] Atualizar documentação

## ✅ SISTEMA DE TAREFAS IMPLEMENTADO COM SUCESSO!

Todas as funcionalidades foram implementadas seguindo a arquitetura Clean Architecture:
- ✅ Entidade Tarefa com regras de negócio
- ✅ Enums StatusTarefa e PrioridadeTarefa
- ✅ DTOs completos para todas as operações
- ✅ Repositório com filtros avançados
- ✅ Serviço de aplicação robusto
- ✅ Validações FluentValidation
- ✅ Controller com 12 endpoints REST
- ✅ Configuração EF Core com índices
- ✅ Injeção de dependências configurada
- ✅ Seed de dados com 8 tarefas de exemplo
- ✅ Documentação completa (README_TAREFAS.md)
- ✅ Multi-tenancy e soft delete
- ✅ Autenticação JWT em todos os endpoints

### Funcionalidades da Entidade Tarefa
- **Título**: Nome da tarefa
- **Descrição**: Detalhes da tarefa
- **Status**: Pendente, Em Andamento, Concluída, Cancelada
- **Prioridade**: Baixa, Média, Alta, Crítica
- **Data de Criação**: Timestamp automático
- **Data de Vencimento**: Prazo opcional
- **Data de Conclusão**: Quando foi marcada como concluída
- **Categoria**: Agrupamento opcional de tarefas
- **TenantId**: Multi-tenancy (herda da BaseEntity)
- **IsDeleted**: Soft delete (herda da BaseEntity)

### Endpoints da API
- `GET /api/tarefa` - Listar tarefas (com filtros)
- `GET /api/tarefa/{id}` - Buscar por ID
- `GET /api/tarefa/search` - Buscar por título/descrição
- `POST /api/tarefa` - Criar nova tarefa
- `PUT /api/tarefa/{id}` - Atualizar tarefa
- `PATCH /api/tarefa/{id}/status` - Alterar status
- `PATCH /api/tarefa/{id}/concluir` - Marcar como concluída
- `DELETE /api/tarefa/{id}` - Excluir (soft delete)

### Filtros de Query String
- `status` - Filtrar por status
- `prioridade` - Filtrar por prioridade
- `categoria` - Filtrar por categoria
- `vencimento` - Filtrar por data de vencimento
- `includeDeleted` - Incluir tarefas excluídas
- `orderBy` - Ordenação (criacao, vencimento, prioridade)

## 🏗️ Estrutura de Arquivos a Criar

### Domain Layer
- `Entities/Tarefa.cs`
- `Enums/StatusTarefa.cs`
- `Enums/PrioridadeTarefa.cs`
- `Interfaces/ITarefaRepository.cs`

### Application Layer
- `DTOs/TarefaDto.cs`
- `DTOs/CreateTarefaDto.cs`
- `DTOs/UpdateTarefaDto.cs`
- `DTOs/UpdateStatusTarefaDto.cs`
- `Interfaces/ITarefaService.cs`
- `Services/TarefaService.cs`
- `Mappings/TarefaMapping.cs`
- `Validators/CreateTarefaDtoValidator.cs`
- `Validators/UpdateTarefaDtoValidator.cs`

### Infrastructure Layer
- `Configurations/TarefaConfiguration.cs`
- `Repositories/TarefaRepository.cs`
- Atualizar `Data/DataSeeder.cs`
- Atualizar `Data/ApplicationDbContext.cs`

### API Layer
- `Controllers/TarefaController.cs`

### IoC Layer
- Atualizar `DependencyInjection.cs`

## 📊 Dados de Seed
Criar tarefas de exemplo para demonstração:
- Tarefas com diferentes status
- Diferentes prioridades
- Algumas com prazo vencido
- Distribuídas entre os tenants existentes

