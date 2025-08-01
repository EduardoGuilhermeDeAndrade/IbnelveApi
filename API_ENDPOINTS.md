# IbnelveApi - Endpoints

## 🔐 Autenticação

### POST /api/auth/register
Registra novo usuário
```json
{
  "email": "teste@exemplo.com",
  "password": "MinhaSenh@123",
  "tenantId": "tenant1"
}
```

### POST /api/auth/login
Realiza login
```json
{
  "email": "admin1@ibnelveapi.com",
  "password": "Admin123!"
}
```

## 👤 Pessoa (Requer Authorization: Bearer {token})

### GET /api/pessoa
Lista todas as pessoas
- Query: `?includeDeleted=true` (opcional)
- Query: `?tenantId=tenant1` (opcional, override)

### GET /api/pessoa/{id}
Busca pessoa por ID

### GET /api/pessoa/cpf/{cpf}
Busca pessoa por CPF

### GET /api/pessoa/search?nome={nome}
Busca pessoas por nome
- Query: `?includeDeleted=true` (opcional)

### POST /api/pessoa
Cria nova pessoa
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

### PUT /api/pessoa/{id}
Atualiza pessoa (mesmo formato do POST)

### DELETE /api/pessoa/{id}
Exclui pessoa logicamente

## 🔑 Usuários de Teste

- **admin1@ibnelveapi.com** / **Admin123!** (tenant1)
- **admin2@ibnelveapi.com** / **Admin123!** (tenant2)

## 📊 Dados de Seed

### Pessoas criadas automaticamente:
- João Silva Santos (CPF: 12345678901, tenant1)
- Maria Oliveira Costa (CPF: 98765432109, tenant2)

## 🚀 Como testar:

1. Execute: `dotnet run` no projeto IbnelveApi.Api
2. Acesse: http://localhost:5000 (Swagger UI)
3. Faça login com um dos usuários admin
4. Use o token retornado nos endpoints de Pessoa
5. Teste os CRUDs e filtros



## 📋 Tarefas (Requer Authorization: Bearer {token})

### GET /api/tarefa
Lista todas as tarefas
- Query: `?includeDeleted=true` (opcional)

### GET /api/tarefa/filtros
Lista tarefas com filtros avançados
- Query: `?status=1&prioridade=3&categoria=Desenvolvimento`
- Query: `?dataVencimentoInicio=2024-01-01&dataVencimentoFim=2024-12-31`
- Query: `?orderBy=prioridade&searchTerm=API&includeDeleted=false`

### GET /api/tarefa/{id}
Busca tarefa por ID

### GET /api/tarefa/search?searchTerm={termo}
Busca tarefas por título ou descrição
- Query: `?includeDeleted=true` (opcional)

### GET /api/tarefa/status/{status}
Lista tarefas por status
- Status: 1=Pendente, 2=EmAndamento, 3=Concluida, 4=Cancelada

### GET /api/tarefa/vencidas
Lista tarefas com prazo vencido

### GET /api/tarefa/concluidas
Lista tarefas concluídas

### POST /api/tarefa
Cria nova tarefa
```json
{
  "titulo": "Implementar nova funcionalidade",
  "descricao": "Desenvolver sistema de notificações",
  "prioridade": 3,
  "dataVencimento": "2024-02-15T00:00:00Z",
  "categoria": "Desenvolvimento"
}
```

### PUT /api/tarefa/{id}
Atualiza tarefa (mesmo formato do POST)

### PATCH /api/tarefa/{id}/status
Altera status da tarefa
```json
{
  "status": 3
}
```

### PATCH /api/tarefa/{id}/concluir
Marca tarefa como concluída

### PATCH /api/tarefa/{id}/pendente
Marca tarefa como pendente

### DELETE /api/tarefa/{id}
Exclui tarefa logicamente

## 📊 Enums de Tarefa

### Status (StatusTarefa)
- 1 = Pendente
- 2 = Em Andamento  
- 3 = Concluída
- 4 = Cancelada

### Prioridade (PrioridadeTarefa)
- 1 = Baixa
- 2 = Média
- 3 = Alta
- 4 = Crítica

## 🎯 Dados de Seed - Tarefas

### Tenant1:
- Implementar autenticação JWT (Em Andamento, Alta)
- Criar documentação da API (Concluída, Média)
- Configurar CI/CD (Pendente, Baixa)
- Revisar código da API (Pendente, Média, Vencida)

### Tenant2:
- Implementar sistema de tarefas (Em Andamento, Crítica)
- Testes unitários (Concluída, Alta)
- Otimizar consultas do banco (Pendente, Média)
- Backup do banco de dados (Pendente, Baixa)

