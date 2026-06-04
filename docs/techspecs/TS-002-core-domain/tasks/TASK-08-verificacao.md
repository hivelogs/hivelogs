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

Padrão (sem PostgreSQL local):

```bash
cd apps/api && dotnet restore && dotnet build && dotnet test
```

Integração opt-in (requer TimescaleDB local):

```bash
cd apps/api && dotnet test --settings test.integration.runsettings --filter "Category=Integration"
```

## Critérios de aceite

- [ ] `dotnet test` verde sem PostgreSQL local
- [ ] `dotnet test --settings test.integration.runsettings --filter "Category=Integration"` documentado como opt-in
- [ ] Smoke manual opcional: POST org → app → env
