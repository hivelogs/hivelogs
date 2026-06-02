---
techspec: TS-001
branch: feature/TS-001-backend-architecture-foundation
ultima_atualizacao: 2026-06-01
tasks_concluidas: [TASK-01, TASK-02, TASK-03, TASK-04, TASK-05, TASK-06, TASK-07, TASK-08, TASK-09]
---

# Shared Memory — TS-001: Fundação da Arquitetura Backend

Documento **vivo** compartilhado entre tasks desta techspec.

## Decisões de implementação

### [TASK-01–09] Implementação completa — 2026-06-01

- Target framework: **net10.0** (SDK 10.0.108).
- Connection string: **`Default`** (alinhado a `infra/.env.example`).
- ADR 003 Accepted — camadas, IoC, erros, Application Services.
- Namespace `HiveLogs.Api.Infrastructure.ProblemDetails` conflita com `Microsoft.AspNetCore.Mvc.ProblemDetails` — usar nome completo no exception handler.
- `GlobalExceptionHandler.MapException` exposto como `internal` + `InternalsVisibleTo` para testes.
- `/health` via Minimal API em `Program.cs`; demais endpoints usarão Controllers.

## Contratos e tipos alterados

| Path | Campo / tipo | Mudança | Task |
|------|--------------|---------|------|
| `HiveLogs.Domain/Common/Errors/` | `Result`, `Result<T>`, `Error`, `ErrorType` | Criados | TASK-03 |
| `HiveLogs.Domain/Common/Errors/GeneralErrors.cs` | Códigos `general.*` | Criados | TASK-03 |
| `HiveLogs.Api/Infrastructure/ResultMapping/` | `ToActionResult` | Criado | TASK-07 |

## APIs e endpoints

| Método | Rota | Payload / response | Task |
|--------|------|-------------------|------|
| GET | `/health` | `{ "status": "healthy", "service": "hivelogs-api" }` | TASK-07 |

## Migrations / schema

| Tabela / objeto | Mudança | Task |
|-----------------|---------|------|
| — | `HiveLogsDbContext` vazio, Npgsql configurado | TASK-05 |

## Gotchas e armadilhas

- Senha em `appsettings.Development.json`: `change-me` — alinhar com `infra/.env`.
- Build pode cachear erros antigos — usar `dotnet clean` se tipos de teste não atualizarem.

## Links

- Techspec: [techspec.md](./techspec.md)
- Planning: [planning.md](./planning.md)
- ADR: [003-backend-clean-architecture-and-error-model.md](../../adr/003-backend-clean-architecture-and-error-model.md)
