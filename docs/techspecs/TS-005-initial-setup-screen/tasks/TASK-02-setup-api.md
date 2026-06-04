---
task_id: TASK-02
techspec: TS-005
titulo: API setup + types + hooks
status: Pending
modulo: web
mvp: MVP 1
depends_on: [TASK-01]
---

# TASK-02 — API setup + types + hooks

## Escopo IN

- `features/setup/types/setup-types.ts`
- `features/setup/api/setup-api.ts`
- `features/setup/api/setup-error-messages.ts`
- `features/setup/hooks/use-setup-status.ts`

## Critérios de aceite

- [ ] GET status e POST initialize via httpClient
- [ ] parseApiError no POST
- [ ] Mapa de mensagens de erro por code
