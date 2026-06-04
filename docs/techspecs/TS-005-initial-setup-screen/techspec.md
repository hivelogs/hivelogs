---
id: TS-005
titulo: Initial Setup Screen
status: Implemented
mvp: MVP 1
branch: feature/TS-005-initial-setup-screen
planning_ref: planning.md
adrs: []
modulos_afetados: [apps/web, docs]
---

# Techspec — TS-005: Initial Setup Screen

## Resumo executivo

Substitui o placeholder `/setup` por tela funcional de configuração inicial self-hosted: bootstrap via `GET /setup/status`, roteamento condicional, formulário com RHF + Zod, `POST /setup/initialize`, tratamento de ProblemDetails e redirecionamento para `/login` com mensagem de setup concluído. UI alinhada a `docs/design.md` e protótipo Pencil (`docs/ui/pencil.pen`). Login real fica para TS-006.

## Regras de negócio confirmadas

- Sem registro público; setup cria primeira organização e primeiro admin (uma vez).
- `setupPassword` informado pelo usuário, vem da config do servidor; frontend só coleta, nunca persiste.
- Senha do admin definida na tela; backend não gera senha.
- Após sucesso → `/login?setupCompleted=true`.
- Se `setupRequired = false` → `/setup` redireciona para `/login`; `/` e `/dashboard` redirecionam para `/login` (sem auth ainda).
- Se `setupRequired = true` → `/setup` exibe formulário; outras rotas principais redirecionam para `/setup`.
- Validação frontend: org min 2, email, senha min 8 + letra + dígito, confirmação igual, setup password obrigatório.
- Erros mapeados: `setup.invalid_setup_password`, `setup.already_completed`, `setup.password_not_configured`, `users.email_already_exists`, validação genérica.
- Senhas não em localStorage, sessionStorage, URL, logs ou mensagens de erro.

## Módulos afetados

| Módulo | Impacto |
|--------|---------|
| apps/web | Feature `setup`, router gate, páginas |
| docs | TS-005, design.md versionado, roadmap, índice |

## ADRs necessários

Nenhum.

## Detalhamento por módulo

### apps/web

**Fazer:**

- `src/features/setup/` — api, types, schema, hooks, components
- `SetupPage` full-screen, `SetupBootstrap` / gate de rotas
- `LoginPlaceholderPage` — mensagem `setupCompleted`
- Copiar logo para assets
- Integrar `parseApiError` no submit

**Não fazer:**

- Login JWT, dashboard real, alterações na API

### docs

**Fazer:**

- Versionar `docs/design.md`, `docs/Logo.svg`, `docs/ui/pencil.pen`
- Pasta TS-005, tasks, shared-memory
- Atualizar índice techspecs e roadmap

## Ordem de implementação

1. TASK-01 — Documentação + design assets
2. TASK-02 — API layer setup
3. TASK-03 — Roteamento bootstrap
4. TASK-04 — Formulário e UI setup
5. TASK-05 — Login placeholder pós-setup
6. TASK-07 — README web + docs
7. TASK-08 — Verificação build/lint

## Impacto em segurança e ingestão

- Afeta API Keys? Não
- Afeta `shared-contracts`? Não
- Reforço: senhas só em memória do formulário; `VITE_API_BASE_URL` apenas

## Critérios de aceite da feature

- [x] `/setup` funcional quando `setupRequired = true`
- [x] `/setup` → `/login` quando `setupRequired = false`
- [x] Zod + RHF; POST initialize; redirect com mensagem em `/login`
- [x] ProblemDetails amigáveis + traceId
- [x] Senhas não persistidas/logadas
- [x] UI conforme design + Pencil
- [x] `docs/design.md` versionado
- [x] `npm run build` e `npm run lint` passam

## Verificação end-to-end

```bash
cd apps/web && npm run build && npm run lint
# API + DB: fluxos setup required / already configured / senha inválida
```

## Tasks previstas

| Task | Módulo | Descrição | depends_on |
|------|--------|-----------|------------|
| TASK-01 | docs | Documentação + design assets | — |
| TASK-02 | web | API setup + types + hooks | TASK-01 |
| TASK-03 | web | SetupBootstrap + router | TASK-02 |
| TASK-04 | web | Schema + form + SetupPage | TASK-03 |
| TASK-05 | web | Login placeholder pós-setup | TASK-04 |
| TASK-07 | docs | README + docs globais | TASK-05 |
| TASK-08 | web | Verificação | TASK-07 |

## Histórico de status

| Status | Data | Notas |
|--------|------|-------|
| Approved | 2026-06-04 | Pronto para implementação |
