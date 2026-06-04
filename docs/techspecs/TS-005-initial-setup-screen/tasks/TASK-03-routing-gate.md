---
task_id: TASK-03
techspec: TS-005
titulo: SetupBootstrap e roteamento
status: Done
modulo: web
mvp: MVP 1
depends_on: [TASK-02]
---

# TASK-03 — SetupBootstrap e roteamento

## Escopo IN

- SetupBootstrap + SetupStatusContext
- router.tsx com redirects condicionais
- Layout full-screen para /setup

## Critérios de aceite

- [x] Loading enquanto status pendente
- [x] setupRequired true → rotas principais → /setup
- [x] setupRequired false → /setup → /login, / → /login
