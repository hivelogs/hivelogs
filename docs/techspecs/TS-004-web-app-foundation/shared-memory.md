---
techspec: TS-004
branch: feature/TS-004-web-app-foundation
ultima_atualizacao: 2026-06-04
tasks_concluidas: []
techspec_status: Approved
---

# Shared Memory — TS-004: Web App Foundation

## Decisões de implementação

- **API URL local:** `http://localhost:5054` (`VITE_API_BASE_URL`)
- **Estrutura:** `src/app`, `src/pages`, `src/shared` (não `src/components`)
- **Home `/`:** texto estático, sem redirect para `/setup`
- **Feature 005:** consumirá `/setup/status` e `/setup/initialize` via `httpClient`

## Contratos e tipos alterados

| Path | Mudança | Task |
|------|---------|------|
| `apps/web/src/shared/api/problem-details.ts` | Tipo ProblemDetails | TASK-05 |
| `apps/web/src/shared/api/api-error.ts` | Classe ApiError | TASK-05 |

## APIs e endpoints

Nenhum consumido nesta feature. Preparado para TS-003:

| Método | Rota | Feature |
|--------|------|---------|
| GET | `/setup/status` | 005 |
| POST | `/setup/initialize` | 005 |

## Gotchas

- Infra usa `WEB_VITE_API_URL`; app web usa `VITE_API_BASE_URL`
- `import.meta.env` exige prefixo `VITE_` no Vite
- Não commitar `.env` com secrets

## Links

- [techspec.md](./techspec.md)
- [planning.md](./planning.md)
- [docs/design.md](../../design.md)
