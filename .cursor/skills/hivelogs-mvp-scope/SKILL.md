---
name: hivelogs-mvp-scope
description: >-
  Classifica features e issues do HiveLogs em MVP 1, 2, 3 ou Future conforme
  docs/roadmap.md e evita scope creep. Use ao planejar issues, PRs ou quando o
  usuário pedir funcionalidade nova.
disable-model-invocation: true
---

# HiveLogs — Escopo MVP

Fonte: [docs/roadmap.md](../../../docs/roadmap.md)

## Classificação

| Fase | Foco |
|------|------|
| **MVP 1 - Foundation** | Auth JWT, orgs/apps/envs, chaves, ingestão básica, dashboard inicial, Docker/TimescaleDB |
| **MVP 2 - SDKs** | SDK .NET/JS, usuários online, heartbeat, agregações, worker retenção, visualizações por ambiente |
| **MVP 3 - MCP** | MCP server, tools leitura, relatórios IA, anomalias, comparação ambientes |
| **Future** | SDK Node, OpenTelemetry, tracing, alertas, SaaS |

## Workflow

1. Identifique a feature pedida.
2. Localize no roadmap (ou marque **Future** se ausente).
3. Se fora do MVP atual, informe o usuário e sugira adiar ou criar issue Future.
4. Use [docs/templates/issue-template.md](../../../docs/templates/issue-template.md) com campo MVP preenchido.

## Scope creep — evitar

Não implementar na mesma tarefa:

- Feature de MVP 2+ quando pedido era MVP 1
- SDK Node.js (Future)
- CI/CD completo (não está no roadmap inicial)
- OpenTelemetry / tracing (Future)

## Ambiguidade

Se fase não estiver clara, **pergunte ao usuário** qual MVP priorizar antes de codar.
