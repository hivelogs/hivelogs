---
id: TS-004
titulo: Web App Foundation
status: Implemented
mvp: MVP 1
branch: feature/TS-004-web-app-foundation
planning_ref: planning.md
adrs: []
modulos_afetados: [apps/web, docs]
---

# Techspec — TS-004: Web App Foundation

## Resumo executivo

Cria o projeto `apps/web` com Vite, React, TypeScript, Tailwind CSS, shadcn/ui, TanStack Query, React Router, ky, Zod e React Hook Form. Entrega shell do app, rotas placeholder (`/`, `/setup`, `/login`, `/dashboard`), providers globais, API client base, tipos `ProblemDetails`/`ApiError`, configuração de env e documentação.

A Feature 005 substituirá o placeholder de `/setup` pela tela funcional consumindo a API da TS-003.

## Regras de negócio confirmadas

- Frontend é interface principal de administração (futuro).
- Nesta feature: apenas fundação; sem consumo de backend.
- `VITE_API_BASE_URL` obrigatório; default local `http://localhost:5054`.
- Nunca secrets em `VITE_*` nem no bundle.
- Tema dark-first alinhado a [docs/design.md](../../design.md).

## Módulos afetados

| Módulo | Impacto |
|--------|---------|
| apps/web | Criar projeto completo |
| docs | techspec, roadmap, architecture, react.mdc, infra nota |

## ADRs necessários

Nenhum — stack já definida em AGENTS.md e architecture.md.

## Detalhamento por módulo

### apps/web

**Fazer:**

- Projeto Vite + React + TS
- Estrutura `src/app`, `src/pages`, `src/shared`
- Tailwind + shadcn (button, card, input, label)
- Providers: QueryClient + Router
- Rotas placeholder
- `httpClient` (ky), `ProblemDetails`, `ApiError`, `env`
- Alias `@` → `src`
- Scripts dev/build/preview/lint

**Não fazer:**

- Setup funcional, login, dashboard real
- Chamadas HTTP reais
- Vitest (opcional futuro)

### docs

**Fazer:**

- Pasta TS-004, tasks, shared-memory
- Atualizar índice techspecs, roadmap, architecture
- Nota infra env naming

## Ordem de implementação

1. TASK-01 — Documentação
2. TASK-02 — Vite projeto + deps
3. TASK-03 — Tailwind + shadcn + tema
4. TASK-04 — Providers + router + páginas
5. TASK-05 — Env + API client + ProblemDetails
6. TASK-06 — Build + lint
7. TASK-07 — README + docs globais
8. TASK-08 — Verificação

## Impacto em segurança e ingestão

- Afeta API Keys? Não
- Afeta `shared-contracts`? Não
- Reforço: apenas `VITE_API_BASE_URL` (URL pública)

## Critérios de aceite da feature

- [ ] `apps/web` criado com estrutura definida
- [ ] `npm install`, `npm run dev`, `npm run build` funcionam
- [ ] `npm run lint` passa
- [ ] Tailwind e shadcn configurados
- [ ] Rotas `/`, `/setup`, `/login`, `/dashboard` acessíveis
- [ ] `VITE_API_BASE_URL` documentada em `.env.example`
- [ ] `httpClient`, `ProblemDetails`, `ApiError` criados
- [ ] Docs TS-004 e índice atualizados

## Verificação end-to-end

```bash
cd apps/web
cp .env.example .env
npm install
npm run build
npm run lint
npm run dev
# Navegar: /, /setup, /login, /dashboard
```

## Tasks previstas

| Task | Módulo | Descrição | depends_on |
|------|--------|-----------|------------|
| TASK-01 | docs | Documentação e techspec | — |
| TASK-02 | web | Vite React TS + deps | TASK-01 |
| TASK-03 | web | Tailwind + shadcn + tema | TASK-02 |
| TASK-04 | web | Providers + router + placeholders | TASK-03 |
| TASK-05 | web | Env + API client + ProblemDetails | TASK-04 |
| TASK-06 | web | Build + lint | TASK-05 |
| TASK-07 | docs | README + docs globais | TASK-06 |
| TASK-08 | web | Verificação final | TASK-07 |

## Histórico de status

| Status | Data | Notas |
|--------|------|-------|
| Draft | 2026-06-04 | Planning |
| Approved | 2026-06-04 | Pronto para implementação |
| Implemented | 2026-06-04 | PR #7 |
