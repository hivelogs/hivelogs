---
id: TS-002
titulo: Core Domain — Organizations, Applications and Environments
mvp: MVP 1
modulos_candidatos: [apps/api, docs]
autor: HiveLogs
data: 2026-06-02
status: ReadyForTechspec
---

# Planning — TS-002: Core Domain

## Problema / objetivo de negócio

O HiveLogs isola dados por **Organization → Application → Environment**. Sem essas entidades persistidas e expostas via API, features futuras (auth, API keys, ingestão) não têm onde ancorar.

**Objetivo:** implementar domínio, persistência, Application Services e endpoints HTTP básicos (criar/consultar) para os três níveis hierárquicos.

## Contexto

- TS-001 entregou fundação backend (ADR 003, Result, ProblemDetails, DbContext vazio).
- Hierarquia documentada em [docs/architecture.md](../../architecture.md) e [docs/security-model.md](../../security-model.md).
- Roadmap MVP 1 lista Organizações, Aplicações e Ambientes como pendentes.

## Restrições

- Manter grafo de dependências da TS-001 (`Api → IoC` apenas).
- Sem auth, users, API keys ou ingestão nesta feature.
- Erros esperados via `Result`; HTTP via `ProblemDetails`.
- ADR 004 para naming `MonitoredApplication` no código.

## Perguntas abertas

> **Gate:** esta seção deve estar vazia (todas respondidas) antes de gerar a techspec.

_(nenhuma)_

## Decisões tomadas

| # | Pergunta | Decisão | Data |
|---|----------|---------|------|
| 1 | Nome da entidade Application no código | `MonitoredApplication` (API/docs: Application) | 2026-06-02 |
| 2 | Limites de nome | Org/App min 2 max 100; Environment min 1 max 64 | 2026-06-02 |
| 3 | Environment names | Lowercase ao persistir; regex `^[a-z0-9_-]+$`; nomes customizados permitidos | 2026-06-02 |
| 4 | EnvironmentKind | Constantes estáticas (development, staging, production); sem enum fechado | 2026-06-02 |
| 5 | Testes API sem Postgres | WebApplicationFactory com EF InMemory | 2026-06-02 |
| 6 | Unicidade Organization name | Global, case-insensitive (`LOWER(name)`) | 2026-06-02 |
| 7 | Unicidade Application name | Por organization, case-insensitive | 2026-06-02 |

## Fora de escopo

- Autenticação, JWT, users, API keys
- Ingestão, dashboard, worker, MCP
- Soft delete, paginação, seeds automáticos
- MediatR, CQRS estrito, Testcontainers

## Riscos e dependências

| Risco / dependência | Impacto | Mitigação |
|---------------------|---------|-----------|
| Conflito `Application` vs camada/namespaces | Confusão no código | ADR 004 + `MonitoredApplication` |
| Conflito `Environment` vs ASP.NET | Erros de compilação | Namespace `HiveLogs.Domain.Environments` |
| Índice `LOWER(name)` no EF | Migration complexa | SQL na migration ou coluna auxiliar |

## Próximo passo

Techspec [techspec.md](./techspec.md) com status `Approved` e tasks TASK-01 … TASK-08.
