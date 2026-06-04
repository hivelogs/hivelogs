# apps/web

## Propósito

Dashboard web do HiveLogs — interface de administração e visualização de observabilidade.

## Responsabilidades

- Consumir a API REST (`apps/api`) para gestão e consulta.
- Exibir métricas, logs, erros e requests (fases futuras).
- Autenticação via JWT (fase futura).
- Stack: React, TypeScript, Vite, Tailwind CSS, shadcn/ui, TanStack Query, React Hook Form, Zod, ky, React Router.

## Estrutura

```
apps/web/
├── src/
│   ├── app/           # App shell, providers, router, estilos globais
│   ├── pages/         # Páginas por rota
│   └── shared/
│       ├── api/       # httpClient, ProblemDetails, ApiError
│       ├── config/    # env (VITE_*)
│       ├── lib/       # utils (cn)
│       └── ui/        # componentes shadcn/ui
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

**Desenvolvimento local:** com `dotnet run` na API, use `http://localhost:5054` ([launchSettings.json](../api/src/HiveLogs.Api/Properties/launchSettings.json)).

**Docker (futuro):** quando o serviço `api` estiver no Compose, a URL costuma ser `http://localhost:8080`.

### Segurança

- `VITE_*` expõe valores no bundle do browser — use **apenas** URLs públicas.
- **Nunca** coloque Backend Secret Key, MCP Access Token, JWT, senhas ou setup password em `VITE_*` ou no código frontend.

## Rotas atuais (TS-004)

| Rota | Estado |
|------|--------|
| `/` | Home — fundação |
| `/setup` | Placeholder (Feature 005: tela funcional de setup) |
| `/login` | Placeholder |
| `/dashboard` | Placeholder |

Esta feature **não** chama `GET /setup/status` nem `POST /setup/initialize`. A Feature 005 implementará o fluxo de setup inicial usando o `httpClient` já preparado em `src/shared/api/`.

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
