---
id: TS-004
titulo: Web App Foundation
mvp: MVP 1
modulos_candidatos: [apps/web, docs]
autor: HiveLogs
data: 2026-06-04
status: Implemented
---

# Planning — TS-004: Web App Foundation

## Problema / objetivo de negócio

O HiveLogs precisa de um dashboard web para administração self-hosted. A TS-003 entregou setup no backend (`GET /setup/status`, `POST /setup/initialize`), mas não existe frontend. Esta feature cria a fundação React/Vite para as próximas telas administrativas, sem implementar ainda o fluxo funcional de setup (Feature 005).

## Contexto

- TS-003: endpoints de setup prontos na API.
- `apps/web` existe apenas com README placeholder.
- Design system documentado em [docs/design.md](../../design.md) (dark-first, paleta cyan/blue/green).
- Stack definida em [AGENTS.md](../../../AGENTS.md): React, TypeScript, Vite, shadcn/ui, TanStack Query, RHF, Zod, ky, React Router.

## Restrições

- Roadmap: [docs/roadmap.md](../../roadmap.md)
- Segurança: [docs/security-model.md](../../security-model.md) — nunca Backend Secret, MCP token ou credenciais em `VITE_*`
- `VITE_API_BASE_URL` aponta para URL pública da API
- API local (`dotnet run`): `http://localhost:5054` ([launchSettings.json](../../../apps/api/src/HiveLogs.Api/Properties/launchSettings.json))
- Infra Docker futuro: porta 8080 — documentar divergência com `WEB_VITE_API_URL` no infra

## Perguntas abertas

> **Gate:** esta seção deve estar vazia (todas respondidas) antes de gerar a techspec.

_(nenhuma)_

## Decisões tomadas

| # | Pergunta | Decisão | Data |
|---|----------|---------|------|
| 1 | URL padrão da API no `.env.example`? | `http://localhost:5054` (alinhado ao `dotnet run` local) | 2026-06-04 |
| 2 | Nome da variável de env? | `VITE_API_BASE_URL` (não `VITE_API_URL` do infra placeholder) | 2026-06-04 |
| 3 | Rota `/` redireciona para `/setup`? | Não; página estática “Web App Foundation” | 2026-06-04 |
| 4 | npm workspaces na raiz? | Não; `apps/web` standalone com npm | 2026-06-04 |
| 5 | Testes (vitest) nesta feature? | Não; fundação primeiro | 2026-06-04 |
| 6 | ADR novo? | Não; segue stack existente | 2026-06-04 |

## Fora de escopo

- Tela funcional de setup (Feature 005)
- Chamadas a `GET /setup/status` e `POST /setup/initialize`
- Login, JWT, dashboard real, sidebar, gráficos
- API Keys, ingestão, Docker production build do web
- npm workspaces na raiz

## Riscos e dependências

| Risco / dependência | Impacto | Mitigação |
|---------------------|---------|-----------|
| Porta API 5054 vs 8080 Docker | Confusão no dev | README e `.env.example` documentam ambos |
| Infra usa `VITE_API_URL` | Nome divergente | Nota em docs; unificar em feature futura |
| shadcn/Tailwind versões | Breaking changes | Fixar em `package-lock.json` |

## Próximo passo

Techspec com status `Approved` → tasks → branch `feature/TS-004-web-app-foundation`.
