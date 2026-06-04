---
task_id: TASK-08
techspec: TS-004
titulo: Verificação final
status: Done
modulo: web
mvp: MVP 1
depends_on: [TASK-07]
---

# TASK-08 — Verificação final

## Critérios de aceite

- [ ] `npm install && npm run build && npm run lint`
- [ ] Rotas `/`, `/setup`, `/login`, `/dashboard` sem erro de env
- [ ] shared-memory e techspec atualizados

## Verificação

```bash
cd apps/web
cp .env.example .env
npm install
npm run build
npm run lint
```
