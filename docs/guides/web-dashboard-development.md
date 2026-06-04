# Guia — Desenvolvimento do dashboard web

Aprendizados de **TS-004** (fundação) e **TS-005** (setup inicial). Use ao implementar novas telas em `apps/web`.

## Pré-requisitos

- API rodando (`dotnet run` em `apps/api`) — ver [api-local-development.md](./api-local-development.md)
- `apps/web/.env` com `VITE_API_BASE_URL=http://localhost:5054`
- [docs/design.md](../design.md) e, se existir, protótipo em [docs/ui/pencil.pen](../ui/pencil.pen)

## Estrutura por feature

Organize código de domínio de tela em `src/features/<nome>/`:

```
src/features/setup/
├── api/           # chamadas HTTP + mapa de erros
├── components/    # formulários e blocos de UI
├── hooks/         # TanStack Query
├── schemas/       # Zod
└── types/         # tipos de request/response
```

Páginas finas em `src/pages/<rota>/` que compõem a feature.

Shared cross-cutting: `src/shared/api/`, `src/shared/ui/`, `src/app/`.

## Consumir a API

### httpClient (ky)

- Base: `src/shared/api/http-client.ts` + `VITE_API_BASE_URL`
- JSON da API ASP.NET Core: **camelCase** nos nomes de propriedade
- Enums de status: strings (`SetupRequired`, `Configured`)

### POST com ProblemDetails

Padrão adotado na TS-005:

```typescript
import { HTTPError } from 'ky'
import { parseApiError } from '@/shared/api/parse-api-error'

try {
  return await httpClient.post('recurso', { json: body }).json<T>()
} catch (error) {
  if (error instanceof HTTPError) {
    throw await parseApiError(error.response)
  }
  throw error
}
```

Trate erros com `ApiError` (`instanceof`) e mapeie `problem.code` para mensagens amigáveis. Exiba `problem.traceId` em texto pequeno para suporte.

### Limitação do backend (validação)

FluentValidation na Application retorna **um** erro por vez no ProblemDetails (não objeto `errors` multi-campo). O Zod no front cobre a maioria dos casos antes do submit.

## Bootstrap de app (ex.: setup)

Fluxo validado na TS-005:

1. No root do router, layout que chama `GET /setup/status` (TanStack Query, key estável ex. `['setup', 'status']`).
2. Enquanto `isPending`: tela de loading full-screen.
3. Se falha de rede: retry explícito (API offline / CORS / URL errada).
4. Gates de rota:
   - `setupRequired === true` → apenas `/setup`; demais rotas → redirect `/setup`
   - `setupRequired === false` → `/setup` → `/login`; `/` → `/login`

Referência: `src/app/routes/SetupBootstrap.tsx`, `SetupGate.tsx`.

`SetupBootstrap` deve renderizar `<Outlet />` (não `children`) para funcionar como layout route do React Router.

## Formulários

- React Hook Form + `@hookform/resolvers/zod`
- Alinhar regras ao validator do backend (ler `*Validator.cs` em `HiveLogs.Application`)
- Campos de senha: `type="password"`, `autoComplete` adequado
- Em erro de API após submit: **não** resetar o formulário inteiro

## Segurança no browser

Checklist obrigatório para telas com credenciais:

| Proibido | Permitido |
|----------|-----------|
| `localStorage` / `sessionStorage` para senhas | Estado do formulário (memória) |
| Senha em query string ou `location` | Redirect com flags não sensíveis (`?setupCompleted=true`) |
| `console.log` de payload com senha | Mensagens de erro genéricas sem ecoar senha |
| `VITE_*` com secrets | Apenas `VITE_API_BASE_URL` |

`setupPassword` e senha de admin: coletar no body do POST, nunca persistir no cliente.

## UI e design

- Tema dark-first: tokens em `src/app/styles/globals.css`
- Telas de onboarding/setup: layout **full-screen**, fora do header genérico de `AppLayout`
- Microcopy do produto em **inglês** até decisão contrária (placeholders e protótipo TS-005)
- Ver [design-handoff-pencil.md](./design-handoff-pencil.md)

## Verificação

```bash
cd apps/web
npm run build
npm run lint
npm run dev
```

Fluxo manual com API + banco: happy path, erro de validação, erro de rede/CORS.

## Referências

- [apps/web/README.md](../../apps/web/README.md)
- [TS-005 shared-memory](../techspecs/TS-005-initial-setup-screen/shared-memory.md)
- Skill: `hivelogs-web-dashboard`
