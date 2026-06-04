---
task_id: TASK-04
techspec: TS-005
titulo: Formulário e SetupPage
status: Pending
modulo: web
mvp: MVP 1
depends_on: [TASK-03]
---

# TASK-04 — Formulário e SetupPage

## Escopo IN

- initial-setup-schema.ts (Zod)
- InitialSetupForm.tsx (RHF)
- SetupPage.tsx (UI Pencil + Setup access)
- logo em assets; remover SetupPlaceholderPage

## Critérios de aceite

- [ ] Todos os campos do spec + setup password
- [ ] Submit POST; erro não limpa form
- [ ] Sucesso → navigate /login?setupCompleted=true
