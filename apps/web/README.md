# apps/web

## Propósito

Dashboard web do HiveLogs — interface de administração e visualização de observabilidade.

## Responsabilidades

- Consumir a API REST (`apps/api`) para gestão e consulta.
- Exibir métricas, logs, erros e requests (fases futuras).
- Autenticação via JWT (TS-006).
- Stack: React, TypeScript, Vite, Tailwind CSS, shadcn/ui, TanStack Query, React Hook Form, Zod, ky, React Router.

## Estrutura

```
apps/web/
├── src/
│   ├── app/              # Shell, providers, router, bootstrap de setup
│   ├── features/
│   │   └── setup/        # Tela de setup inicial (TS-005)
│   ├── pages/            # Páginas por rota
│   └── shared/
│       ├── api/          # httpClient, ProblemDetails, ApiError
│       ├── config/       # env (VITE_*)
│       ├── lib/          # utils (cn)
│       └── ui/           # componentes shadcn/ui
├── .env.example
└── package.json
```

## Como rodar

```bash
cd apps/web
cp .env.example .env
npm install
npm run dev
```

Com a API local (`dotnet run` em `apps/api`) e banco disponível:

```bash
# Terminal 1 — API (porta 5054)
cd apps/api && dotnet run --project src/HiveLogs.Api

# Terminal 2 — Web
cd apps/web && npm run dev
```

Outros scripts:

```bash
npm run build    # typecheck + bundle de produção
npm run preview  # preview do build
npm run lint     # ESLint
```

O dev server padrão do Vite fica em `http://localhost:5173`.

## Variáveis de ambiente

| Variável | Obrigatória | Exemplo | Descrição |
|----------|-------------|---------|-----------|
| `VITE_API_BASE_URL` | Sim | `http://localhost:5054` | URL pública da API (sem barra final) |

**Setup (API):** configure `HIVELOGS_SETUP_PASSWORD` ou `Setup:Password` no servidor — o frontend apenas solicita esse valor na tela de setup; **nunca** persiste no browser.

**Desenvolvimento local:** com `dotnet run` na API, use `http://localhost:5054` ([launchSettings.json](../api/src/HiveLogs.Api/Properties/launchSettings.json)).

### Segurança

- `VITE_*` expõe valores no bundle do browser — use **apenas** URLs públicas.
- **Nunca** coloque Backend Secret Key, MCP Access Token, JWT, senhas de admin ou setup password em `VITE_*` ou no código frontend.
- Senhas de formulário existem só em memória (React Hook Form); não usar `localStorage` / `sessionStorage`.

## Fluxo de setup inicial (TS-005)

1. Ao abrir o app, `GET /setup/status` define o roteamento.
2. Se `setupRequired = true` → `/setup` exibe o formulário (organização, admin, setup password).
3. Submit → `POST /setup/initialize` → redireciona para `/login?setupCompleted=true`.
4. Se `setupRequired = false` → `/setup` e `/` redirecionam para `/login`.

Login real: **TS-006** (substitui placeholder de `/login`).

## Rotas

| Rota | Comportamento |
|------|----------------|
| `/setup` | Formulário de setup (se pendente) ou redirect para `/login` |
| `/login` | Placeholder; mensagem se setup acabou de concluir |
| `/dashboard` | Placeholder (após setup configurado) |
| `/` | Redirect para `/login` ou `/setup` conforme status |

## Design

- [docs/design.md](../../docs/design.md) — design system dark-first
- [docs/ui/pencil.pen](../../docs/ui/pencil.pen) — protótipo da tela de setup

## O que NÃO deve conter

- Backend Secret Key ou MCP Access Token.
- Lógica de ingestão de telemetria.
- Endpoints ou regras de negócio duplicadas da API.
- Processamento assíncrono ou agregações (use `apps/worker`).

## Referências

- [docs/architecture.md](../../docs/architecture.md)
- [docs/security-model.md](../../docs/security-model.md)
- [docs/design.md](../../docs/design.md)
- [docs/techspecs/TS-004-web-app-foundation/](../../docs/techspecs/TS-004-web-app-foundation/)
- [docs/techspecs/TS-005-initial-setup-screen/](../../docs/techspecs/TS-005-initial-setup-screen/)
