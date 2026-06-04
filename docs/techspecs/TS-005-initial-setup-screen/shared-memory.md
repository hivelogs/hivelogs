---
techspec: TS-005
branch: feature/TS-005-initial-setup-screen
ultima_atualizacao: 2026-06-04
tasks_concluidas: [TASK-01, TASK-02, TASK-03, TASK-04, TASK-05, TASK-07, TASK-08]
---

# Shared Memory — TS-005: Initial Setup Screen

Documento **vivo** compartilhado entre tasks desta techspec.

**Leitura obrigatória** antes de implementar qualquer task.

## Decisões de implementação

### [TASK-04] Formulário e SetupPage — 2026-06-04

- `SetupBootstrap` usa `Outlet`; query única `['setup','status']`.
- `getSetupErrorMessage` usa `instanceof ApiError`.
- Login preserva mensagem de setup com `useState` antes de limpar query string.

- Layout `/setup`: full-screen, fora do `AppLayout` header.
- UI: inglês; CTA "Create instance" (Pencil).
- Protótipo Pencil disponível em `docs/ui/pencil.pen` (frame `kZpjm`).
- Campo `setupPassword`: seção "Setup access" (requisito TS-003, ausente no Pencil).
- Query key: `['setup', 'status']`.
- Sem link "Back to sign in" durante `setupRequired`.

## Contratos e tipos alterados

| Path | Campo / tipo | Mudança | Task |
|------|--------------|---------|------|
| `features/setup/types/setup-types.ts` | SetupStatusResponse, InitializeSetupRequest | Novo | TASK-02 |

## APIs e endpoints

| Método | Rota | Payload / response | Task |
|--------|------|-------------------|------|
| GET | `/setup/status` | `{ status, setupRequired }` | TASK-02 |
| POST | `/setup/initialize` | body camelCase; 201 response sem senhas | TASK-02 |

## Dependências entre tasks

- TASK-03 depende de `useSetupStatus` (TASK-02).
- TASK-04 depende do router gate (TASK-03).

## Gotchas e armadilhas

- Backend retorna primeiro erro de validação FluentValidation (não objeto `errors` multi-campo).
- `setup.invalid_setup_password` → HTTP 401.
- Organization name: min 2 chars no backend.

## Links

- Techspec: [techspec.md](./techspec.md)
- Planning: [planning.md](./planning.md)
- TS-003: [../TS-003-self-hosted-setup-access-model/](../TS-003-self-hosted-setup-access-model/)
- TS-004: [../TS-004-web-app-foundation/](../TS-004-web-app-foundation/)
