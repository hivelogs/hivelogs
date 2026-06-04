---
task_id: TASK-04
techspec: TS-004
titulo: Configurar providers, router e páginas placeholder
status: Pending
modulo: web
mvp: MVP 1
depends_on: [TASK-03]
---

# TASK-04 — Configurar providers, router e páginas placeholder

## Escopo IN

- `providers.tsx`: QueryClient (retry false, refetchOnWindowFocus false) + RouterProvider
- `router.tsx`: `/`, `/setup`, `/login`, `/dashboard`
- Páginas placeholder com textos do spec

## Critérios de aceite

- [ ] 4 rotas navegáveis
- [ ] Sem chamadas HTTP
