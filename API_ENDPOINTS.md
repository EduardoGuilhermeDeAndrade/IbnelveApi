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

