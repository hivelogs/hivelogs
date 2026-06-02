---
task_id: TASK-08
techspec: TS-002
titulo: Verificação final
status: Done
modulo: api
mvp: MVP 1
depends_on: [TASK-07]
---

# TASK-08 — Verificação final

## Verificação

```bash
cd apps/api && dotnet restore && dotnet build && dotnet test
```

## Critérios de aceite

- [ ] Build e testes verdes
- [ ] Smoke manual opcional: POST org → app → env
