---
name: hivelogs-web-dashboard
description: >-
  Implementa features do dashboard HiveLogs em apps/web: estrutura features/,
  ky + ProblemDetails, TanStack Query, RHF + Zod, gates de rota e segurança
  de formulários. Use ao criar telas web, consumir API, setup/login ou corrigir
  erros CORS/parseApiError no frontend.
disable-model-invocation: true
---

# HiveLogs — Web Dashboard

Guia completo: [docs/guides/web-dashboard-development.md](../../../docs/guides/web-dashboard-development.md)

## Antes de codar

1. Ler contratos reais na API (`Controllers`, `Application/*/Requests`, `Validators`) — **não inventar JSON**.
2. Confirmar `VITE_API_BASE_URL` e CORS na API ([api-local-development.md](../../../docs/guides/api-local-development.md)).
3. Se houver UI: [design.md](../../../docs/design.md) + [design-handoff-pencil.md](../../../docs/guides/design-handoff-pencil.md).

## Estrutura

```
src/features/<feature>/
  api/ hooks/ schemas/ types/ components/
src/pages/<route>/<Page>.tsx
src/app/routes/   # gates e layouts
```

## API client

- GET: `httpClient.get('path').json<T>()`
- POST mutação: capturar `HTTPError` → `parseApiError(response)` → relançar `ApiError`
- Mapear `problem.code` para mensagens; mostrar `traceId` opcional

Query keys estáveis: ex. `['setup', 'status']`.

## Rotas condicionais (padrão setup)

1. Layout root: fetch status + loading/erro de rede + `<Outlet />`
2. `RequireSetupPending` / `RequireSetupComplete` com `<Navigate replace />`
3. Telas de onboarding: **sem** header do `AppLayout`

## Formulários

- Zod alinhado ao validator .NET (min length, regex senha, etc.)
- Senhas: `type="password"`; nunca storage/URL/logs
- Erro API: não resetar form inteiro
- Mutation que muda estado de gate global: `setQueryData` + `invalidateQueries` **antes** de `navigate`

## CORS — diagnóstico rápido

| Sintoma | Ação |
|---------|------|
| Falha só no browser | Adicionar origem Vite em `Cors:AllowedOrigins` (5173 **e** 5174) |
| Falha em curl | API down ou URL errada — não é CORS |

## Verificação

```bash
cd apps/web && npm run build && npm run lint
```

## Escopo

- Não implementar JWT/login real sem techspec
- Não colocar secrets em `VITE_*`
