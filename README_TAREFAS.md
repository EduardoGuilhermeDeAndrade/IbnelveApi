# IbnelveApi - Sistema de Gestão de Tarefas

## 🎯 Visão Geral

O sistema de gestão de tarefas foi implementado seguindo a mesma arquitetura Clean Architecture do projeto IbnelveApi, mantendo consistência e qualidade do código. O sistema oferece funcionalidades completas para criação, edição, acompanhamento e organização de tarefas.

## ✨ Funcionalidades Implementadas

### 🔧 Funcionalidades Core
- ✅ **CRUD Completo**: Criar, listar, atualizar e excluir tarefas
- ✅ **Multi-tenancy**: Isolamento de dados por tenant
- ✅ **Soft Delete**: Exclusão lógica com possibilidade de recuperação
- ✅ **Autenticação JWT**: Proteção de todos os endpoints
- ✅ **Validação Robusta**: FluentValidation em todos os DTOs

### 📊 Gestão de Status
- **Pendente**: Tarefa criada, aguardando início
- **Em Andamento**: Tarefa sendo executada
- **Concluída**: Tarefa finalizada com sucesso
- **Cancelada**: Tarefa cancelada

### 🎯 Níveis de Prioridade
- **Baixa**: Tarefas não urgentes
- **Média**: Prioridade padrão
- **Alta**: Tarefas importantes
- **Crítica**: Máxima prioridade (requer categoria)

### 🔍 Filtros e Buscas Avançadas
- Filtrar por status, prioridade, categoria
- Buscar por título ou descrição
- Filtrar por data de vencimento
- Listar tarefas vencidas
- Listar tarefas concluídas
- Incluir/excluir registros deletados
- Ordenação customizável

## 🏗️ Arquitetura Implementada

### Domain Layer
```
📁 Entities/
  └── Tarefa.cs                 # Entidade principal com regras de negócio
📁 Enums/
  ├── StatusTarefa.cs          # Enum para status das tarefas
  └── PrioridadeTarefa.cs      # Enum para prioridades
📁 Interfaces/
  └── ITarefaRepository.cs     # Interface do repositório
```

### Application Layer
```
📁 DTOs/
  └── TarefaDto.cs             # DTOs para todas as operações
📁 Interfaces/
  └── ITarefaService.cs        # Interface do serviço
📁 Services/
  └── TarefaService.cs         # Lógica de aplicação
📁 Mappings/
  └── TarefaMapping.cs         # Mapeamentos manuais
📁 Validators/
  ├── CreateTarefaDtoValidator.cs
  └── UpdateTarefaDtoValidator.cs
```

### Infrastructure Layer
```
📁 Repositories/
  └── TarefaRepository.cs      # Implementação do repositório
📁 Configurations/
  └── TarefaConfiguration.cs   # Configuração EF Core
📁 Data/
  └── DataSeeder.cs           # Dados de exemplo (atualizado)
```

### API Layer
```
📁 Controllers/
  └── TarefaController.cs      # Endpoints REST completos
```

## 🌐 Endpoints da API

### Autenticação
Todos os endpoints requerem autenticação JWT:
```
Authorization: Bearer {seu-token-jwt}
```

### Endpoints Principais

#### **GET /api/tarefa**
Lista todas as tarefas do tenant
- Query: `?includeDeleted=true` (opcional)

#### **GET /api/tarefa/filtros**
Lista tarefas com filtros avançados
- Query: `?status=1&prioridade=3&categoria=Desenvolvimento`
- Query: `?dataVencimentoInicio=2024-01-01&dataVencimentoFim=2024-12-31`
- Query: `?orderBy=prioridade&searchTerm=API`

#### **GET /api/tarefa/{id}**
Busca tarefa específica por ID

#### **GET /api/tarefa/search?searchTerm={termo}**
Busca tarefas por título ou descrição

#### **GET /api/tarefa/status/{status}**
Lista tarefas por status (1=Pendente, 2=EmAndamento, 3=Concluida, 4=Cancelada)

#### **GET /api/tarefa/vencidas**
Lista tarefas com prazo vencido

#### **GET /api/tarefa/concluidas**
Lista tarefas concluídas

#### **POST /api/tarefa**
Cria nova tarefa
```json
{
  "titulo": "Implementar nova funcionalidade",
  "descricao": "Desenvolver sistema de notificações em tempo real",
  "prioridade": 3,
  "dataVencimento": "2024-02-15T00:00:00Z",
  "categoria": "Desenvolvimento"
}
```

#### **PUT /api/tarefa/{id}**
Atualiza tarefa completa

#### **PATCH /api/tarefa/{id}/status**
Altera apenas o status
```json
{
  "status": 3
}
```

#### **PATCH /api/tarefa/{id}/concluir**
Marca tarefa como concluída

#### **PATCH /api/tarefa/{id}/pendente**
Marca tarefa como pendente

#### **DELETE /api/tarefa/{id}**
Exclui tarefa (soft delete)

## 📋 Modelos de Dados

### TarefaDto (Response)
```json
{
  "id": 1,
  "titulo": "Implementar autenticação JWT",
  "descricao": "Configurar sistema de autenticação usando JWT Bearer tokens",
  "status": 2,
  "statusDescricao": "Em Andamento",
  "prioridade": 3,
  "prioridadeDescricao": "Alta",
  "dataVencimento": "2024-02-07T00:00:00Z",
  "dataConclusao": null,
  "categoria": "Desenvolvimento",
  "tenantId": "tenant1",
  "createdAt": "2024-01-31T10:00:00Z",
  "updatedAt": "2024-01-31T15:30:00Z",
  "estaVencida": false,
  "estaConcluida": false
}
```

### CreateTarefaDto (Request)
```json
{
  "titulo": "string (3-200 chars)",
  "descricao": "string (5-1000 chars)",
  "prioridade": "1-4 (Baixa, Media, Alta, Critica)",
  "dataVencimento": "datetime (opcional, não pode ser passado)",
  "categoria": "string (opcional, max 100 chars)"
}
```

## 🔧 Validações Implementadas

### Título
- Obrigatório
- Mínimo 3 caracteres
- Máximo 200 caracteres

### Descrição
- Obrigatória
- Mínimo 5 caracteres
- Máximo 1000 caracteres

### Prioridade
- Deve ser valor válido do enum
- Tarefas críticas devem ter categoria

### Data de Vencimento
- Não pode ser anterior a hoje
- Campo opcional

### Categoria
- Máximo 100 caracteres
- Campo opcional

## 🗄️ Dados de Exemplo

O sistema inclui dados de seed automático:

### Tenant1 (admin1@ibnelveapi.com)
- Implementar autenticação JWT (Em Andamento, Alta)
- Criar documentação da API (Concluída, Média)
- Configurar CI/CD (Pendente, Baixa)
- Revisar código da API (Pendente, Média, Vencida)

### Tenant2 (admin2@ibnelveapi.com)
- Implementar sistema de tarefas (Em Andamento, Crítica)
- Testes unitários (Concluída, Alta)
- Otimizar consultas do banco (Pendente, Média)
- Backup do banco de dados (Pendente, Baixa)

## 🚀 Como Testar

### 1. Executar a aplicação
```bash
cd IbnelveApi.Api
dotnet run
```

### 2. Acessar Swagger
Navegue para: `http://localhost:5000`

### 3. Fazer login
```json
POST /api/auth/login
{
  "email": "admin1@ibnelveapi.com",
  "password": "Admin123!"
}
```

### 4. Usar o token
Copie o token retornado e use no cabeçalho:
```
Authorization: Bearer {token}
```

### 5. Testar endpoints
- Liste tarefas: `GET /api/tarefa`
- Crie nova tarefa: `POST /api/tarefa`
- Filtre por status: `GET /api/tarefa/status/1`
- Busque tarefas: `GET /api/tarefa/search?searchTerm=API`

## 📊 Exemplos de Uso

### Listar tarefas pendentes
```bash
GET /api/tarefa/status/1
```

### Buscar tarefas de desenvolvimento
```bash
GET /api/tarefa/filtros?categoria=Desenvolvimento
```

### Listar tarefas vencidas
```bash
GET /api/tarefa/vencidas
```

### Criar tarefa crítica
```bash
POST /api/tarefa
{
  "titulo": "Corrigir bug crítico",
  "descricao": "Sistema está fora do ar, correção urgente necessária",
  "prioridade": 4,
  "dataVencimento": "2024-02-01T18:00:00Z",
  "categoria": "Bugfix"
}
```

### Marcar como concluída
```bash
PATCH /api/tarefa/1/concluir
```

## 🎯 Benefícios da Implementação

### ✅ Consistência Arquitetural
- Seguiu exatamente o mesmo padrão do sistema de Pessoa
- Manteve a estrutura Clean Architecture
- Reutilizou componentes existentes (ApiResponse, BaseEntity)

### ✅ Funcionalidades Avançadas
- Sistema completo de filtros e buscas
- Gestão de status e prioridades
- Controle de prazos e vencimentos
- Categorização flexível

### ✅ Qualidade de Código
- Validações robustas com FluentValidation
- Mapeamentos manuais otimizados
- Tratamento de erros consistente
- Documentação completa via Swagger

### ✅ Escalabilidade
- Multi-tenancy nativo
- Soft delete para auditoria
- Índices otimizados no banco
- Estrutura extensível para novas funcionalidades

O sistema de tarefas está completamente integrado ao projeto IbnelveApi e pronto para uso em produção! 🚀

