# 🧪 Prova Prática

Bem-vindo(a)! Esta é sua prova prática para a vaga de Desenvolvedor .NET. A ideia é simular um desafio realista do dia a dia de desenvolvimento.

---

## 📦 Desafio: Cadastro e Consulta de Produtos

Você deverá desenvolver uma API REST para gerenciamento de produtos. Essa API será usada para manter o catálogo de produtos de um e-commerce.

### Funcionalidades obrigatórias:

- Cadastrar um novo produto
- Editar produto existente
- Excluir um produto
- Consultar lista de produtos com filtros:
  - Por categoria
  - Por faixa de preço
  - Por status (Ativo/Inativo)
  - Upload de imagem do produto
  - Simular envio para a AWS S3 (pode ser salvo em disco ou usar MinIO local), ou algum outro similar.

---

## 🛠️ Requisitos Técnicos

- .NET 6 ou superior
- API REST
- Usar alguma arquitetura, por exemplo em camadas
- Banco de dados relacional (PostgreSQL)
- Documentando os endpoints, por exemplo com o Swagger
- Testes em pelo menos uma parte da regra de negócio

---

## ✨ Diferenciais (Bônus)

Estes itens não são obrigatórios, mas contam pontos na avaliação:

- CI/CD (ex: GitHub Actions para build/test)
- Diagrama da arquitetura ou documentação da estrutura do código
- Docker (com docker-compose subindo app e banco)

---

## ✅ Critérios de Avaliação

- Clareza e organização do código
- Uso adequado de OOP e boas práticas (SOLID, Clean Code)
- Estrutura dos endpoints e convenções REST
- Cobertura e qualidade dos testes
- Commits claros e bem organizados
- Facilidade de execução do projeto

---

## Arquitetura e Estrutura do Código

O projeto segue arquitetura em camadas, separando responsabilidades entre apresentação, aplicação, domínio e infraestrutura.

### Visão em camadas

- **Presentation**: Controllers HTTP e middleware global de exceções.
- **Application**: Casos de uso, contratos e DTOs para entrada/saída da API.
- **Domain**: Entidades, regras de negócio, enums, exceções e contratos de repositório.
- **Infrastructure**: Persistência com EF Core/PostgreSQL e armazenamento local de imagens.

### Estrutura principal de pastas

- `Presentation/`
  - `Controllers/ProductsController.cs`
  - `Middlewares/GlobalExceptionMiddleware.cs`
- `Application/`
  - `DTOs/` (requests/responses)
  - `Interfaces/` (contratos de serviço/armazenamento)
  - `Services/ProductService.cs`
- `Domain/`
  - `Entities/Product.cs`
  - `Enums/ProductStatus.cs`
  - `Exceptions/DomainException.cs`
  - `Repositories/IProductRepository.cs`
- `Infrastructure/`
  - `Persistence/AppDbContext.cs`
  - `Persistence/Repositories/ProductRepository.cs`
  - `Storage/LocalStorageService.cs`
- `TesteTecnico.Tests/`
  - Testes unitários de regras de domínio da entidade `Product`

---

## 🧪 Testes

Para executar apenas os testes unitários implementados no projeto:

Escopo atual dos testes: regras de domínio de `Product` (nome e categoria obrigatórios, preço maior que zero e normalização de textos com `trim`).

```bash
dotnet test "TesteTecnico.Tests/TesteTecnico.Tests.csproj" -v minimal
```

---

## Rodando sem Docker

### Pré-requisitos

- .NET SDK 8.0+
- PostgreSQL 16+ em execução local

### 1) Criar banco e tabela localmente

Crie o banco `product_catalog` no seu PostgreSQL e execute o script SQL abaixo (o mesmo usado no bootstrap do Docker):

```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS products (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  name VARCHAR(200) NOT NULL,
  description TEXT NOT NULL,
  category VARCHAR(150) NOT NULL,
  price NUMERIC(18,2) NOT NULL CHECK (price > 0),
  status INTEGER NOT NULL,
  image_url VARCHAR(500),
  created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
  updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

### 2) Validar connection string

No arquivo `appsettings.json`, ajuste `ConnectionStrings:DefaultConnection` para sua instância local (usuário/senha/porta).

### 3) Restaurar dependências e executar

```bash
dotnet restore
dotnet run --project TesteTecnico.csproj
```

Após subir a API:

- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`

---

## Rodando utilizando Docker

Para subir a API e o PostgreSQL com Docker Compose:

```bash
docker compose up --build -d
```

Após subir os containers:

- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`

Para parar os containers:

```bash
docker compose down
```

Observação: o PostgreSQL executa o script de inicialização em `docker/db/init/01-init-products.sql` apenas na primeira criação do volume.
Se precisar recriar banco/tabelas do zero:

```bash
docker compose down -v
docker compose up --build -d
```

---

## 🚀 Como Entregar

1. Faça um **fork deste repositório** ou clone e crie um repositório público seu.
2. Desenvolva a prova no seu repositório.
3. Inclua no seu README instruções claras para rodar o projeto localmente.
4. Quando finalizar, envie o link do seu repositório para a pessoa responsável pelo processo.

---

## ⏰ Prazo

Conforme e-mail encaminhado com o link do desafio.

---

Boa sorte! 💻🚀
