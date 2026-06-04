---
id: TS-005
titulo: Initial Setup Screen
mvp: MVP 1
modulos_candidatos: [apps/web, docs]
autor: HiveLogs
data: 2026-06-04
status: Implemented
---

# Planning — TS-005: Initial Setup Screen

## Problema / objetivo de negócio

Após TS-003 (API de setup) e TS-004 (fundação web), o administrador self-hosted ainda vê apenas placeholders. Esta feature entrega a primeira experiência real: consultar status do setup, preencher organização e admin, concluir configuração inicial e ser direcionado ao login (placeholder até TS-006).

## Contexto

- Backend: `GET /setup/status`, `POST /setup/initialize` (TS-003).
- Frontend: Vite, React, TanStack Query, RHF, Zod, ky, shadcn (TS-004).
- Design: [docs/design.md](../../design.md) (dark-first).
- Protótipo: [docs/ui/pencil.pen](../../ui/pencil.pen) — frame "Initial Setup Screen".

## Restrições

- Roadmap: [docs/roadmap.md](../../roadmap.md)
- Segurança: [docs/security-model.md](../../security-model.md) — `setupPassword` e senhas de admin nunca em storage, URL ou logs
- Contratos alinhados a `SetupController` e DTOs em `HiveLogs.Application.Setup`
- UI em inglês (placeholders TS-004 e protótipo Pencil)

## Perguntas abertas

> **Gate:** esta seção deve estar vazia (todas respondidas) antes de gerar a techspec.

_(nenhuma)_

## Decisões tomadas

| # | Pergunta | Decisão | Data |
|---|----------|---------|------|
| 1 | Idioma da UI? | Inglês | 2026-06-04 |
| 2 | Layout `/setup`? | Full-screen dedicado, sem header do `AppLayout` | 2026-06-04 |
| 3 | Botão principal? | "Create instance" (protótipo Pencil) | 2026-06-04 |
| 4 | Campo setup password? | Seção "Setup access" (não no Pencil; obrigatório TS-003) | 2026-06-04 |
| 5 | Pós-setup redirect? | `/login?setupCompleted=true` | 2026-06-04 |
| 6 | Login real? | Fora de escopo (TS-006) | 2026-06-04 |
| 7 | Testes Vitest? | Opcional; validação manual prioritária | 2026-06-04 |
| 8 | Alterar backend? | Não, salvo bug documentado | 2026-06-04 |

## Fora de escopo

- Login real, JWT, sessão, `auth/me`
- Dashboard real, sidebar, gráficos
- API Keys, ingestão, SDKs, MCP
- Registro público, reset de senha do único admin
- Alteração de contratos da API

## Riscos e dependências

| Risco / dependência | Impacto | Mitigação |
|---------------------|---------|-----------|
| API offline no bootstrap | App sem rota útil | Loading + mensagem de rede genérica |
| Protótipo sem setup password | Gap visual | Seção "Setup access" documentada em shared-memory |
| Backend retorna um erro de validação por vez | UX de campo | Mostrar `detail` + `code`; Zod cobre maioria no cliente |

## Próximo passo

Techspec `Approved` → tasks → `feature/TS-005-initial-setup-screen`.
