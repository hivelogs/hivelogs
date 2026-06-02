---
techspec: TS-002
branch: feature/TS-002-core-domain
ultima_atualizacao: 2026-06-02
tasks_concluidas: [TASK-01, TASK-02, TASK-03, TASK-04, TASK-05, TASK-06, TASK-07, TASK-08]
techspec_status: Approved
nota_merge: Techspec permanece Approved até merge do PR; status Implemented após merge (workflow).
---

# Shared Memory — TS-002: Core Domain

Documento **vivo** compartilhado entre tasks desta techspec.

## Decisões de implementação

- **MonitoredApplication:** entidade em `HiveLogs.Domain.Applications`; rotas/DTOs usam Application. Ver [ADR 004](../../adr/004-core-domain-monitored-application.md).
- **Endpoints:** 9 REST aninhados (3 por recurso: POST create, GET list, GET by id); sem autenticação; erros via `Result` + ProblemDetails (`organizations.*`, `applications.*`, `environments.*`).
- **Unicidade de nomes:** `organizations` e `applications` — case-insensitive via coluna computada `name_lower` (`lower(name)`, stored) + índice único; `environments` — nome normalizado no domínio (lowercase) + UX `(application_id, name)`.
- **Rotas aninhadas:** services validam vínculo org ↔ app ↔ env antes de operar no filho.
- **Repositórios:** implementação EF `internal` com `InternalsVisibleTo` para `HiveLogs.Infrastructure.Tests`.
- **Testes InMemory:** `HiveLogs.Api.Tests` usa `Testing:UseInMemoryDatabase` no `WebApplicationFactory`; `HiveLogs.Application.Tests` usa repositórios in-memory em `TestDoubles/`; integração de persistência real em `CoreDomainPersistenceTests` (Postgres local, senha default `hivelogs` do compose).
- **IoC / grafo:** `HiveLogs.Api.csproj` referencia somente `HiveLogs.IoC` (ADR 003); controllers usam tipos de `HiveLogs.Application` via referência transitiva de compilação. `CompositionRootTests` valida `AddHiveLogsDependencies` (3 services + 3 repos).

## Contratos e tipos alterados

| Path | Campo / tipo | Mudança | Task |
|------|--------------|---------|------|
| `HiveLogs.Domain` | `Organization`, `MonitoredApplication`, `Environment` | Entidades + value objects + erros | TASK-02 |
| `HiveLogs.Application` | `*Service`, Requests/Responses/Validators | Application Services por contexto | TASK-03 |

## APIs e endpoints

| Método | Rota | Payload / response | Task |
|--------|------|-------------------|------|
| POST | `/organizations` | `{ "name" }` → `OrganizationResponse` | TASK-05 |
| GET | `/organizations` | `OrganizationResponse[]` | TASK-05 |
| GET | `/organizations/{organizationId}` | `OrganizationResponse` | TASK-05 |
| POST | `/organizations/{organizationId}/applications` | `{ "name" }` → `ApplicationResponse` | TASK-05 |
| GET | `/organizations/{organizationId}/applications` | `ApplicationResponse[]` | TASK-05 |
| GET | `/organizations/{organizationId}/applications/{applicationId}` | `ApplicationResponse` | TASK-05 |
| POST | `.../environments` | `{ "name" }` → `EnvironmentResponse` | TASK-05 |
| GET | `.../environments` | `EnvironmentResponse[]` | TASK-05 |
| GET | `.../environments/{environmentId}` | `EnvironmentResponse` | TASK-05 |

## Migrations / schema

| Tabela / objeto | Mudança | Task |
|-----------------|---------|------|
| `organizations` | `id`, `name`, `name_lower` (computed), `created_at`, `updated_at`; UX `ix_organizations_name_lower` | TASK-04 |
| `applications` | FK `organization_id`; `name_lower` (computed); UX `(organization_id, name_lower)` | TASK-04 |
| `environments` | FK `application_id`; UX `(application_id, name)` | TASK-04 |
| Migration `InitialCoreDomain` | `20260602214111_InitialCoreDomain`; `dotnet ef database update --project src/HiveLogs.Infrastructure --startup-project src/HiveLogs.Api` | TASK-04 |

## Gotchas e armadilhas

- `Environment` no Domain pode exigir namespace qualificado onde coexistir com tipos ASP.NET.
- Duplicata de nome de org/app é 409 (comparação via `name_lower`); environment inválido no body → 400.
- Testes de API não exigem Postgres; testes de Infrastructure com Postgres precisam do container.

## Links

- Techspec: [techspec.md](./techspec.md)
- Planning: [planning.md](./planning.md)
- ADR: [004-core-domain-monitored-application.md](../../adr/004-core-domain-monitored-application.md)
- README API: [apps/api/README.md](../../../apps/api/README.md)
