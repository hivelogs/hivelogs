# AGENTS.md — HiveLogs

Instruções para agentes de código (Cursor, Copilot, etc.) trabalhando neste repositório.

Documentação para usuários finais: [README.md](README.md) (inglês).

## Visão do projeto

HiveLogs é uma plataforma open source de observabilidade e analytics self-hosted. Monorepo multi-serviço com ingestão via API, dashboard React, worker assíncrono e servidor MCP para análises por IA.

| Documento | Conteúdo |
|-----------|----------|
| [docs/architecture.md](docs/architecture.md) | Módulos, fluxos, conceitos de domínio |
| [docs/security-model.md](docs/security-model.md) | JWT, API Keys, rate limiting |
| [docs/roadmap.md](docs/roadmap.md) | MVP 1, 2, 3 e Future |
| [docs/adr/](docs/adr/) | Decisões arquiteturais |
| [docs/techspecs/](docs/techspecs/) | Techspecs, tasks e shared-memory por feature |

## Fluxo de feature (planning → techspec → tasks)

```
Nova feature
  → planning.md              (dúvidas resolvidas)
  → techspec.md              (status Approved; ADR se necessário)
  → tasks/ + shared-memory.md
  → branch feature/TS-NNN-slug
  → 1 commit por task + atualizar shared-memory.md
  → 1 PR único → main
  → techspec status Implemented
```

Orquestração: skill **`hivelogs-feature-workflow`**. Detalhes: [docs/techspecs/README.md](docs/techspecs/README.md).

## Política de idiomas

| Audiência | Idioma |
|-----------|--------|
| README raiz, docs de usuário final | Inglês |
| `docs/`, ADRs, READMEs de módulos, `AGENTS.md`, skills | Português (PT-BR) |

## Mapa do monorepo

| Caminho | Responsabilidade | Não contém |
|---------|------------------|------------|
| `apps/api` | Ingestão, JWT, API keys, persistência | UI, worker pesado, MCP tools |
| `apps/web` | Dashboard React | Ingestão, Backend Secret |
| `apps/worker` | Agregações, retenção | HTTP ingestão, UI |
| `apps/mcp` | Tools MCP, leitura para IA | Ingestão, UI |
| `packages/sdk-dotnet` | Logs/erros/requests .NET | Frontend key, UI |
| `packages/sdk-js` | Eventos/erros browser | Backend Secret |
| `packages/shared-contracts` | Schemas, enums, tipos | Regras de negócio |
| `infra/` | Docker Compose, env | Código de app |

## Hierarquia de dados

```
Organization → Application → Environment
```

Cada Environment tem suas próprias chaves e dados isolados.

## Regras inegociáveis

1. **Frontend Public Key** — pública; só ingestão browser; nunca escopos de leitura/admin.
2. **Backend Secret Key** — só servidor; nunca em `apps/web`, JS bundle ou `VITE_*`.
3. **MCP Access Token** — secreta; preferir read-only; revogável; sem ingestão.
4. **JWT** — dashboard apenas; nunca em SDKs de ingestão.
5. Novos payloads → `packages/shared-contracts` antes da implementação.
6. Dúvidas de regra de negócio → perguntar ao usuário.
7. Escopo mínimo — não implementar features de MVP futuro sem alinhamento.

## Stack

| Camada | Tecnologia |
|--------|------------|
| Backend | .NET (estável), Clean Architecture |
| Banco | PostgreSQL + TimescaleDB |
| Frontend | React, TypeScript, Vite, shadcn/ui, TanStack Query, RHF, Zod |
| Local | Docker Compose em `infra/` |

## Comandos

### Banco (disponível hoje)

```bash
cd infra
cp .env.example .env
docker compose up timescaledb -d
```

### API / Web / Worker / MCP (após inicialização dos projetos)

```bash
cd apps/api
dotnet restore && dotnet build && dotnet test
dotnet run --project src/HiveLogs.Api
# GET /health → {"status":"healthy","service":"hivelogs-api"}
```

```bash
# Web — a documentar após npm create vite
# Worker — a documentar após dotnet new worker
# MCP — a documentar após init do projeto
```

## Padronização Cursor

| Recurso | Caminho |
|---------|---------|
| Regras sempre ativas | `.cursor/rules/core.mdc` |
| Regras .NET | `.cursor/rules/dotnet.mdc` |
| Regras React | `.cursor/rules/react.mdc` |
| Regras docs | `.cursor/rules/docs.mdc` |
| Skills do projeto | `.cursor/skills/` |
| Templates | `docs/templates/` |

## Skills do projeto

Invoque pelo nome (`name` no frontmatter) ou descreva o cenário.

| Skill | Use quando |
|-------|------------|
| `hivelogs-context` | Início de qualquer tarefa; carregar boundaries |
| `hivelogs-feature-workflow` | Feature nova end-to-end; orquestração do fluxo |
| `hivelogs-feature-planning` | Planejamento, dúvidas de negócio, `planning.md` |
| `hivelogs-techspec` | Techspec por módulo; status Approved |
| `hivelogs-task-breakdown` | Quebrar techspec em tasks + `shared-memory.md` |
| `hivelogs-task-implement` | Implementar task; 1 commit; PR único ao final |
| `hivelogs-adr` | Criar ou revisar decisão arquitetural |
| `hivelogs-module-scaffold` | Novo app/package ou README de módulo |
| `hivelogs-security-review` | Auth, chaves, ingestão, env, SDKs, PR sensível |
| `hivelogs-mvp-scope` | Classificar feature; evitar scope creep |

## Templates

Copie de [docs/templates/](docs/templates/):

- `feature-planning-template.md` → `docs/techspecs/TS-NNN/planning.md`
- `techspec-template.md` → `docs/techspecs/TS-NNN/techspec.md`
- `task-template.md` → `docs/techspecs/TS-NNN/tasks/TASK-NN.md`
- `shared-memory-template.md` → `docs/techspecs/TS-NNN/shared-memory.md`
- `adr-template.md` → `docs/adr/NNN-titulo.md`
- `issue-template.md` → corpo de issue
- `pr-description-template.md` → descrição de PR (1 PR por techspec)
- `module-readme-template.md` → README de módulo

## Roadmap

Fase atual alvo: **MVP 1 - Foundation**. Antes de implementar feature nova, consulte [docs/roadmap.md](docs/roadmap.md) ou use `hivelogs-mvp-scope`.

## Skills externas (opcional)

Não são obrigatórias; o repo é autocontido.

| Skill | Uso |
|-------|-----|
| `vibesec-skill` | Auditoria de segurança web |
| `verification-before-completion` (superpowers) | Evidência antes de marcar tarefa concluída |
| `npx skills find [query]` | Descobrir skills da comunidade em [skills.sh](https://skills.sh/) |

## Antes de concluir uma tarefa

- [ ] Boundaries do módulo respeitados (`hivelogs-context`)
- [ ] Sem vazamento de secrets (`hivelogs-security-review` se aplicável)
- [ ] Escopo alinhado ao MVP (`hivelogs-mvp-scope` se feature nova)
- [ ] Feature seguiu fluxo planning → techspec → tasks (`hivelogs-feature-workflow`)
- [ ] `shared-memory.md` atualizado após implementação de task
- [ ] Docs/ADR atualizados se decisão ou boundary mudou
