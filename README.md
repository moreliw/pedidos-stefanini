# Pedidos Stefanini API

API REST para gerenciamento de pedidos desenvolvida em .NET 8, seguindo princípios de Clean Architecture, DDD e SOLID.

## 📋 Funcionalidades

- ✅ CRUD completo de Pedidos
- ✅ Listagem de Produtos (para criação de pedidos)
- ✅ Validações com FluentValidation
- ✅ Documentação automática com Swagger
- ✅ Migrations automáticas do Entity Framework
- ✅ Testes unitários com xUnit
- ✅ Arquitetura limpa e modular

## 🏗️ Arquitetura do Projeto

```
src/
├── Pedidos.Domain/          # Entidades de domínio (Pedido, ItemPedido, Produto)
├── Pedidos.Application/     # DTOs, interfaces e serviços de aplicação
├── Pedidos.Infrastructure/  # EF Core, repositórios e configurações de dados
└── Pedidos.API/            # Controllers, middleware e configurações da API

tests/
└── Pedidos.UnitTests/      # Testes unitários com Moq e FluentAssertions
```

## 🚀 Como Executar Localmente

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) ou SQL Server

### 1. Clonar e Restaurar Dependências

```bash
# Clonar o repositório
git clone <url-do-repositorio>
cd pedidos-stefanini

# Restaurar pacotes NuGet
dotnet restore PedidosStefanini.sln
```

### 2. Configurar Banco de Dados

O projeto está configurado para usar SQL Server LocalDB por padrão. A connection string está em:

- `src/Pedidos.API/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PedidosStefaniniDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

**Para usar outro SQL Server**, altere a connection string conforme necessário.

### 3. Executar Migrations

```bash
# Aplicar migrations e criar o banco
dotnet ef database update -p src/Pedidos.Infrastructure -s src/Pedidos.API
```

### 4. Executar a Aplicação

```bash
# Compilar o projeto
dotnet build PedidosStefanini.sln

# Executar a API
dotnet run --project src/Pedidos.API/Pedidos.API.csproj
```

A API estará disponível em:

- **HTTP**: `http://localhost:5012`
- **HTTPS**: `https://localhost:7012`
- **Swagger**: `http://localhost:5012/swagger`

## 📚 Endpoints da API

### Pedidos

- `GET /api/pedidos` - Listar todos os pedidos
- `GET /api/pedidos/{id}` - Obter pedido por ID
- `POST /api/pedidos` - Criar novo pedido
- `PUT /api/pedidos/{id}` - Atualizar pedido
- `DELETE /api/pedidos/{id}` - Excluir pedido

### Produtos

- `GET /api/produtos` - Listar produtos disponíveis

### Health Check

- `GET /health` - Verificar status da API

## 🧪 Executar Testes

```bash
# Executar todos os testes
dotnet test tests/Pedidos.UnitTests/Pedidos.UnitTests.csproj

# Executar com relatório de cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 📝 Exemplo de Uso

### Criar um Pedido

```bash
POST /api/pedidos
Content-Type: application/json

{
  "nomeCliente": "João Silva",
  "emailCliente": "joao@email.com",
  "itensPedido": [
    {
      "produtoId": 1,
      "quantidade": 2
    },
    {
      "produtoId": 2,
      "quantidade": 1
    }
  ]
}
```

### Resposta

```json
{
  "id": 1,
  "nomeCliente": "João Silva",
  "emailCliente": "joao@email.com",
  "dataCriacao": "2024-11-03T10:30:00",
  "valorTotal": 2500.0,
  "itensPedido": [
    {
      "id": 1,
      "produtoId": 1,
      "nomeProduto": "Notebook Dell",
      "valorUnitario": 1200.0,
      "quantidade": 2,
      "valorTotal": 2400.0
    },
    {
      "id": 2,
      "produtoId": 2,
      "nomeProduto": "Mouse Logitech",
      "valorUnitario": 100.0,
      "quantidade": 1,
      "valorTotal": 100.0
    }
  ]
}
```

## 🗄️ Dados Iniciais (Seeds)

O projeto inclui produtos pré-cadastrados para facilitar os testes:

1. **Notebook Dell** - R$ 1.200,00
2. **Mouse Logitech** - R$ 100,00
3. **Teclado Mecânico** - R$ 300,00

## 🔧 Configurações Adicionais

### Alterar Porta da API

Edite o arquivo `src/Pedidos.API/Properties/launchSettings.json`:

```json
{
  "profiles": {
    "http": {
      "applicationUrl": "http://localhost:5000"
    },
    "https": {
      "applicationUrl": "https://localhost:5001;http://localhost:5000"
    }
  }
}
```

### Configurar CORS (se necessário)

O projeto já está configurado com CORS aberto para desenvolvimento. Para produção, ajuste em `src/Pedidos.API/Program.cs`.

## 🛠️ Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server** - Banco de dados
- **FluentValidation** - Validações
- **Swagger/OpenAPI** - Documentação da API
- **xUnit** - Framework de testes
- **Moq** - Mock para testes
- **FluentAssertions** - Assertions para testes

## 📖 Padrões Implementados

- **Clean Architecture** - Separação clara de responsabilidades
- **Domain Driven Design (DDD)** - Modelagem focada no domínio
- **Repository Pattern** - Abstração do acesso a dados
- **Unit of Work** - Controle de transações
- **SOLID Principles** - Código limpo e manutenível

**Desenvolvido para o desafio técnico Stefanini** 🚀
