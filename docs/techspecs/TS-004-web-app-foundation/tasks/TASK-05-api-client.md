---
task_id: TASK-05
techspec: TS-004
titulo: Criar env config, API client e ProblemDetails base
status: Done
modulo: web
mvp: MVP 1
depends_on: [TASK-04]
---

# TASK-05 — Criar env config, API client e ProblemDetails base

## Escopo IN

- `env.ts`, `.env.example` (5054)
- `http-client.ts` (ky)
- `problem-details.ts`, `api-error.ts`
- `parseApiError` helper (sem uso em páginas)

## Critérios de aceite

- [ ] `VITE_API_BASE_URL` obrigatório
- [ ] httpClient com prefixUrl e timeout 30s
