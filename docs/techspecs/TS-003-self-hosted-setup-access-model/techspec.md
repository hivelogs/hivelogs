---
id: TS-003
titulo: Self-hosted Setup and Access Model
status: Implemented
mvp: MVP 1
branch: feature/TS-003-self-hosted-setup-access-model
planning_ref: planning.md
adrs: [005-self-hosted-setup-and-access-model.md]
modulos_afetados: [apps/api, docs]
---

# Techspec — TS-003: Self-hosted Setup and Access Model

## Resumo executivo

Fundação de acesso self-hosted: entidades `User`, `OrganizationMember`, `SetupState`; setup inicial protegido por `Setup:Password`; endpoints `GET /setup/status` e `POST /setup/initialize`; hash de senha via `IPasswordHasher`. Sem login/JWT.

## Regras de negócio confirmadas

- Sem registro público.
- Estados: `SetupRequired` / `Configured` via `setup_state`.
- Setup password: env `HIVELOGS_SETUP_PASSWORD` ou `Setup:Password`; não altera banco após setup.
- Admin inicial: `UserRole.Admin`, `MustChangePassword = false`, membership `Owner`.
- Senha mínima: 8+ chars, 1 letra, 1 número.
- Erros: `setup.*`, `users.*` via `Result` + ProblemDetails.
- `setup.invalid_setup_password` → **401 Unauthorized**.

## Módulos afetados

| Módulo | Impacto |
|--------|---------|
| apps/api | Domain, Application, Infrastructure, API, testes |
| docs | ADR 005, architecture, security-model, roadmap |

## Endpoints

| Método | Rota | Auth |
|--------|------|------|
| GET | `/setup/status` | Público |
| POST | `/setup/initialize` | Setup password no body |

## Schema

| Tabela | Colunas principais |
|--------|-------------------|
| `users` | id, name, email, email_lower, password_hash, role, must_change_password, is_active, created_at, updated_at |
| `organization_members` | id, organization_id, user_id, role, created_at |
| `setup_state` | id, is_completed, completed_at |

## Ordem de implementação

1. TASK-01 — Documentação (ADR, planning, techspec, tasks, índice)
2. TASK-02 — Domain + testes
3. TASK-03 — Application + testes
4. TASK-04 — Infrastructure + migration + testes
5. TASK-05 — API + testes
6. TASK-06 — IoC e config
7. TASK-07 — README e docs globais
8. TASK-08 — Verificação (`dotnet build` / `dotnet test`)

## Decisões finais (code review)

- `SetupState.SingletonId` fixo; PK impede múltiplas linhas.
- `GET /setup/status` read-only (sem `SaveChanges` quando não há estado).
- `POST /setup/initialize` atômico (um `SaveChanges` no final).
- `IDatabaseExceptionClassifier` mapeia apenas unique violation em `setup_state` → `setup.already_completed`.
- Setup password: `SHA256.HashData` + `FixedTimeEquals`.

## Critérios de aceite

- `dotnet build` e `dotnet test` verdes sem PostgreSQL local
- Setup pendente → `SetupRequired`
- Setup válido → org + admin + membership + `Configured`
- Setup repetido → 409
- Response sem senhas/hash
- ADR 005 e docs atualizados
