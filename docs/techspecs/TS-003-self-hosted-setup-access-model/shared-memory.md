---
techspec: TS-003
branch: feature/TS-003-self-hosted-setup-access-model
ultima_atualizacao: 2026-06-04
tasks_concluidas: [TASK-01, TASK-02, TASK-03, TASK-04, TASK-05, TASK-06, TASK-07, TASK-08]
techspec_status: Approved
---

# Shared Memory — TS-003: Self-hosted Setup and Access Model

## Decisões de implementação

- **SetupState singleton:** ID fixo `SetupState.SingletonId` (`00000000-0000-0000-0000-000000000001`); PK impede múltiplas linhas.
- **GET /setup/status:** read-only; sem `setup_state` persistido retorna `SetupRequired` sem `SaveChanges`.
- **POST /setup/initialize:** cria org + admin + membership + `setup_state` completed em um único `SaveChangesAsync`; conflito de PK/concorrência → `setup.already_completed`.
- **Setup password inválido:** HTTP 401 (`setup.invalid_setup_password`).
- **Setup password ausente na config:** HTTP 500 (`setup.password_not_configured`).
- **Hash:** `PasswordHasher<PasswordUser>` (ASP.NET Core Identity package, sem Identity completo).
- **Comparação setup password:** comparação em tempo constante via `CryptographicOperations.FixedTimeEquals` sobre UTF-8 bytes de comprimento igual (hash SHA256 dos valores se comprimentos diferem — ou pad; usar approach: hash both and compare hashes to avoid length leak).
- **Admin inicial:** `UserRole.Admin`, `OrganizationMemberRole.Owner`, `MustChangePassword = false`.
- **Email:** value object `Email` com `email_lower` único (shadow/computed como org names).
- **Testes:** InMemory padrão; `HiveLogsWebApplicationFactory` define `Setup:Password` para testes.

## Contratos e tipos alterados

| Path | Mudança | Task |
|------|---------|------|
| `HiveLogs.Domain/Users` | User, Email, UserName, PasswordHash, UserRole | TASK-02 |
| `HiveLogs.Domain/Setup` | SetupState, SetupStatus | TASK-02 |
| `HiveLogs.Domain/OrganizationMembers` | OrganizationMember | TASK-02 |

## APIs e endpoints

| Método | Rota | Task |
|--------|------|------|
| GET | `/setup/status` | TASK-05 |
| POST | `/setup/initialize` | TASK-05 |

## Migrations / schema

| Migration | Task |
|-------------|------|
| `AddSelfHostedAccessModel` | TASK-04 |

## Gotchas

- Não logar `setupPassword` nem `adminPassword`.
- `POST /setup/initialize` retorna 201 com body (não CreatedAtAction — sem recurso único de rota).
- Reiniciar app não reexecuta setup se `setup_state.is_completed`.

## Links

- [techspec.md](./techspec.md)
- [ADR 005](../../adr/005-self-hosted-setup-and-access-model.md)
