---
id: TS-002
titulo: Core Domain — Organizations, Applications and Environments
status: Approved
mvp: MVP 1
branch: feature/TS-002-core-domain
planning_ref: planning.md
adrs: [004-core-domain-monitored-application.md]
modulos_afetados: [apps/api, docs]
---

# Techspec — TS-002: Core Domain

## Resumo executivo

Implementa a hierarquia **Organization → MonitoredApplication → Environment** no backend: entidades de domínio, repositórios, Application Services, EF Core com migration inicial, controllers REST e testes por camada. Sem autenticação.

Ordem: documentação → Domain → Application → Infrastructure → API → IoC → docs módulo → verificação.

## Regras de negócio confirmadas

- Organization: nome obrigatório (2–100), unicidade global case-insensitive.
- MonitoredApplication: pertence a Organization; nome único por org (case-insensitive).
- Environment: pertence a Application; nome lowercase (1–64), regex `^[a-z0-9_-]+$`, único por application.
- Rotas aninhadas validam vínculo org ↔ app ↔ env.
- Erros: `organizations.*`, `applications.*`, `environments.*` via `Result` + ProblemDetails.

## Módulos afetados

| Módulo | Impacto | Observação |
|--------|---------|------------|
| apps/api | Sim | Domínio completo |
| docs | Sim | ADR 004, architecture, roadmap |
| apps/web | Não | — |
| packages/* | Não | — |

## ADRs necessários

| ADR | Status | Ação |
|-----|--------|------|
| [004](../../adr/004-core-domain-monitored-application.md) | Accepted | MonitoredApplication naming |

## Endpoints

| Método | Rota |
|--------|------|
| POST | `/organizations` |
| GET | `/organizations` |
| GET | `/organizations/{organizationId}` |
| POST | `/organizations/{organizationId}/applications` |
| GET | `/organizations/{organizationId}/applications` |
| GET | `/organizations/{organizationId}/applications/{applicationId}` |
| POST | `/organizations/{organizationId}/applications/{applicationId}/environments` |
| GET | `/organizations/{organizationId}/applications/{applicationId}/environments` |
| GET | `/organizations/{organizationId}/applications/{applicationId}/environments/{environmentId}` |

## Ordem de implementação

1. TASK-01 — Documentação
2. TASK-02 — Domain + testes
3. TASK-03 — Application + testes
4. TASK-04 — Infrastructure + migration + testes
5. TASK-05 — API controllers + testes
6. TASK-06 — IoC
7. TASK-07 — README e docs
8. TASK-08 — Verificação final

## Critérios de aceite

- `dotnet build` e `dotnet test` verdes em `apps/api`
- `GET /health` inalterado
- 9 endpoints REST conforme spec
- Migration `InitialCoreDomain` aplicável via `dotnet ef database update`
