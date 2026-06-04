---
name: hivelogs-ef-migrations
description: >-
  Cria e aplica migrations EF Core do HiveLogs no projeto Infrastructure:
  pasta Migrations/, EfMigrationsDirectory, comandos dotnet ef e armadilhas de
  histórico. Use ao alterar HiveLogsDbContext, schema PostgreSQL ou corrigir
  migrations em pasta errada.
disable-model-invocation: true
---

# HiveLogs — EF Migrations

Guia completo: [docs/guides/api-local-development.md](../../../docs/guides/api-local-development.md)

## Regras

| Regra | Detalhe |
|-------|---------|
| Pasta | `apps/api/src/HiveLogs.Infrastructure/Migrations/` apenas |
| Nunca | `Persistence/Migrations/` — consolidar e remover duplicatas |
| csproj | `<EfMigrationsDirectory>Migrations</EfMigrationsDirectory>` |
| ID | Não renomear migration já aplicada (`__EFMigrationsHistory`) |

## Comandos

```bash
cd apps/api

dotnet ef migrations add DescricaoClara \
  --project src/HiveLogs.Infrastructure \
  --startup-project src/HiveLogs.Api

dotnet ef database update \
  --project src/HiveLogs.Infrastructure \
  --startup-project src/HiveLogs.Api
```

## Checklist pós `migrations add`

- [ ] Arquivos gerados em `Infrastructure/Migrations/`, não em `Persistence/`
- [ ] Namespace `HiveLogs.Infrastructure.Migrations`
- [ ] `HiveLogsDbContextModelSnapshot.cs` atualizado na mesma pasta
- [ ] README `apps/api` atualizado se migration de domínio relevante

## Problemas comuns

| Erro | Mitigação |
|------|-----------|
| `already exists` no update | Índice/tabela criada manualmente — alinhar DB ou migration |
| Migration em pasta errada | Mover para `Migrations/`, corrigir namespace, manter `[Migration("id")]` |
| Designer vazio em migration antiga | OK se snapshot central estiver correto; não reescrever histórico sem ADR |

## Testes de integração

Testes em `HiveLogs.Infrastructure.Tests` usam `Database.MigrateAsync()` — migrations devem estar consistentes para CI/local com Postgres.
