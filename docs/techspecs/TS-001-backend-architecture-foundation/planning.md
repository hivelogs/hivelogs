---
id: TS-001
titulo: Fundação da Arquitetura Backend
mvp: MVP 1
modulos_candidatos: [apps/api, docs]
autor: HiveLogs
data: 2026-06-01
status: ReadyForTechspec
---

# Planning — TS-001: Fundação da Arquitetura Backend

## Problema / objetivo de negócio

O backend do HiveLogs ainda não possui solution .NET nem padrões de camadas. Sem essa fundação, features de domínio (organizações, auth, ingestão) nasceriam com acoplamento e inconsistência.

**Objetivo:** criar a base arquitetural em `apps/api` — projetos, dependências, Result/ProblemDetails, `/health`, testes e documentação — **sem** implementar regras de negócio reais.

## Contexto

- Monorepo com `apps/api` placeholder ([README](../../../apps/api/README.md)).
- ADR 001 define monorepo + Clean Architecture; ADR 002 define dimensões de telemetria.
- Infra local: TimescaleDB via Docker Compose; connection string `Default` em `.env.example`.

## Restrições

- Roadmap: [docs/roadmap.md](../../roadmap.md) — MVP 1 Foundation.
- Segurança: sem secrets reais versionados.
- ADR obrigatório: [ADR 003](../../adr/003-backend-clean-architecture-and-error-model.md).
- Fluxo: planning → techspec Approved → tasks → implementação.

## Perguntas abertas

> **Gate:** esta seção deve estar vazia (todas respondidas) antes de gerar a techspec.

_(nenhuma)_

## Decisões tomadas

| # | Pergunta | Decisão | Data |
|---|----------|---------|------|
| 1 | ADR formal para padrões backend? | Sim — ADR 003 | 2026-06-01 |
| 2 | Nome da connection string | `Default` (alinhado a infra) | 2026-06-01 |
| 3 | CQRS/MediatR no MVP? | Não — Application Services | 2026-06-01 |

## Fora de escopo

- Auth/JWT, Users, Organizations/Applications/Environments reais
- API Keys, ingestão, migrations de domínio
- MediatR, AutoMapper, Domain Events, Outbox
- Worker, Web, MCP, SDKs, CI/CD, OpenTelemetry

## Riscos e dependências

| Risco / dependência | Impacto | Mitigação |
|---------------------|---------|-----------|
| SDK .NET 10 vs LTS 8/9 | Build local | `Directory.Build.props` com TF alvo do SDK instalado |
| Teste de 500 sem endpoint público | Cobertura de middleware | Teste unitário do exception handler |

## Próximo passo

Gerar [techspec.md](./techspec.md) com status `Approved` e quebrar em tasks.
