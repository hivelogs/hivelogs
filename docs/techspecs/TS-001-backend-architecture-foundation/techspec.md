---
id: TS-001
titulo: Fundação da Arquitetura Backend
status: Implemented
mvp: MVP 1
branch: feature/TS-001-backend-architecture-foundation
planning_ref: planning.md
adrs: [003-backend-clean-architecture-and-error-model.md]
modulos_afetados: [apps/api, docs]
---

# Techspec — TS-001: Fundação da Arquitetura Backend

## Resumo executivo

Esta techspec entrega a fundação do backend em `apps/api`: solution .NET com cinco projetos de produção (`Api`, `IoC`, `Application`, `Domain`, `Infrastructure`), quatro projetos de teste, padrão Result + ProblemDetails, middleware global de exceções, endpoint `GET /health` e documentação do módulo.

Não há domínio de negócio implementado — apenas estrutura, contratos de erro e DbContext vazio preparado para PostgreSQL/Npgsql.

Ordem de execução: artefatos e ADR → solution scaffold → Domain → Application → Infrastructure → IoC → Api → testes → README.

## Regras de negócio confirmadas

- Nenhuma regra de negócio de Organization/Application/Environment nesta feature.
- `/health` retorna 200 sem depender do banco.
- Erros esperados usam `Result`; falhas inesperadas usam exceptions → `ProblemDetails`.
- Api referencia somente IoC.

## Módulos afetados

| Módulo | Impacto | Observação |
|--------|---------|------------|
| apps/api | Sim | Solution completa |
| apps/web | Não | — |
| apps/worker | Não | — |
| apps/mcp | Não | — |
| packages/* | Não | — |
| infra | Não | Apenas documentar alinhamento `Default` |
| docs | Sim | ADR-003, techspec, tasks |

## ADRs necessários

| ADR | Status | Ação |
|-----|--------|------|
| [003](../../adr/003-backend-clean-architecture-and-error-model.md) | Accepted | Criado — camadas, IoC, erros, Application Services |

## Detalhamento por módulo

### apps/api

**Fazer:**

- `HiveLogs.Api.sln` e projetos conforme ADR 003
- Domain: Result, Error, entities base, DomainException
- Application: estrutura por contexto, FluentValidation DI, abstractions
- Infrastructure: HiveLogsDbContext vazio, SystemClock, Npgsql
- IoC: `AddHiveLogsDependencies`
- Api: controllers pattern, `/health`, exception handling, Result → ProblemDetails
- Testes mínimos por camada
- README do módulo (PT-BR)

**Não fazer:**

- Entidades de negócio, auth, ingestão, migrations reais

**Arquivos / pastas previstas:**

- `apps/api/src/HiveLogs.*`
- `apps/api/tests/HiveLogs.*.Tests`
- `apps/api/Directory.Build.props`
- `apps/api/README.md`

## Ordem de implementação

1. TASK-01 — Documentação (planning, techspec, ADR, tasks)
2. TASK-02 — Solution scaffold
3. TASK-03 — Domain
4. TASK-04 — Application
5. TASK-05 — Infrastructure
6. TASK-06 — IoC
7. TASK-07 — Api
8. TASK-08 — Testes
9. TASK-09 — README + dotnet.mdc + shared-memory

## Critérios de aceite globais

```bash
cd apps/api
dotnet restore && dotnet build && dotnet test
dotnet run --project src/HiveLogs.Api
# GET /health → {"status":"healthy","service":"hivelogs-api"}
```

- Grafo de dependências conforme ADR 003
- Sem MediatR, AutoMapper, CQRS libs

## Verificação

Ver tasks individuais em [tasks/](./tasks/).
