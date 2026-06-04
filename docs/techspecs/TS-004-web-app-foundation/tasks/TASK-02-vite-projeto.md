---
task_id: TASK-02
techspec: TS-004
titulo: Criar projeto apps/web com Vite React TypeScript
status: Done
modulo: web
mvp: MVP 1
depends_on: [TASK-01]
---

# TASK-02 — Criar projeto apps/web com Vite React TypeScript

## Escopo IN

- `npm create vite` react-ts em `apps/web`
- Dependências: react-router-dom, @tanstack/react-query, ky, zod, react-hook-form, @hookform/resolvers, cva, clsx, tailwind-merge, lucide-react
- Alias `@` → `src` em vite e tsconfig

## Escopo OUT

- Tailwind, shadcn, páginas (tasks seguintes)

## Critérios de aceite

- [ ] `package.json` com name e deps
- [ ] `npm install` sem erro
- [ ] Alias `@/` funcional

## Verificação

```bash
cd apps/web && npm install
```
