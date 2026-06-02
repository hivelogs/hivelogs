# ADR 003 - Backend Clean Architecture e Modelo de Erros

## Status

Accepted

## Context

O backend do HiveLogs (`apps/api`) precisa de uma fundação arquitetural antes de implementar domínio de negócio (organizações, autenticação, ingestão). Sem padrões claros de camadas, injeção de dependências, tratamento de erros e contratos HTTP, cada feature subsequente tenderia a acoplar regras de negócio à API, misturar erros esperados com exceptions e dificultar testes.

O monorepo já define Clean Architecture e .NET no [ADR 001](001-monorepo-and-stack.md), mas não detalha a organização interna da API, o composition root nem o modelo de erros na borda HTTP.

## Decision

Adotar **DDD pragmático + Clean Architecture + projeto IoC separado** em `apps/api`:

### Grafo de dependências

```
Api → IoC
IoC → Application, Infrastructure
Infrastructure → Application, Domain
Application → Domain
Domain → (nenhuma referência interna)
```

- `HiveLogs.Api` referencia **somente** `HiveLogs.IoC`.
- `HiveLogs.IoC` é o **composition root** (`AddHiveLogsDependencies`).
- A API não registra EF, repositórios ou detalhes de infraestrutura diretamente.

### DDD pragmático (MVP 1)

**Usar:** Entities, Aggregates quando necessário, Value Objects quando fizer sentido, Enums, Domain Errors, Domain Exceptions, invariantes.

**Evitar no MVP inicial:** Domain Services prematuros, abstrações sem uso real, Domain Events, CQRS estrito, MediatR.

### Application layer

Organização por **contexto funcional** com Application Services (`I<Context>Service`, `<Context>Service`, Requests, Responses, Validators). Commands/Queries/Handlers só serão introduzidos se reduzirem complexidade no futuro.

### Modelo de erros

| Tipo | Mecanismo | Uso |
|------|-----------|-----|
| Erro esperado | `Result` / `Result<T>` + `Error` | Not found, conflito, validação, credenciais inválidas |
| Falha inesperada | Exception (`DomainException` ou genérica) | Bug, config ausente, falha de infra não controlada |
| Contrato HTTP | `ProblemDetails` | Respostas de erro da API com `code`, `traceId`, `type` |

Mapeamento HTTP mínimo:

| ErrorType | Status |
|-----------|--------|
| Validation | 400 |
| NotFound | 404 |
| Conflict | 409 |
| Unauthorized | 401 |
| Forbidden | 403 |
| Failure | 500 |

Convenção de códigos: `<context>.<reason>` (ex.: `organizations.not_found`, `general.unexpected`).

### Connection string

Nome da chave: **`Default`**, alinhado a `infra/.env.example` e placeholders do Docker Compose.

## Consequences

### Positivas

- Boundaries claros para features 002+ (domínio, auth, ingestão).
- Erros esperados testáveis sem exceptions.
- Composition root único facilita testes e evolução de infraestrutura.
- ADR referenciável por agentes e contribuidores.

### Negativas

- Mais projetos na solution que um monólito único (custo aceitável para clareza).
- Disciplina necessária para não referenciar Infrastructure a partir da Api.

## Alternatives Considered

### CQRS estrito com MediatR desde o MVP

**Rejeitado:** cerimônia excessiva sem casos de uso reais ainda; Application Services cobrem o MVP 1.

### Registro de DI direto em `Program.cs` (sem IoC)

**Rejeitado:** Api passaria a conhecer tipos de Infrastructure; viola Clean Architecture na borda.

### Exceptions para todos os erros de negócio

**Rejeitado:** dificulta fluxo esperado (404, 409) e mistura controle de fluxo com falhas técnicas.

### Connection string `HiveLogs`

**Rejeitado nesta feature:** manter `Default` para alinhar com `infra/` existente.
