# Techspecs do HiveLogs

Especificações técnicas de features, quebradas em tasks autocontidas. Fluxo completo: skill `hivelogs-feature-workflow` ou [AGENTS.md](../../AGENTS.md).

## Fluxo resumido

```
Nova feature
  → planning.md          (dúvidas resolvidas)
  → techspec.md          (status Approved)
  → tasks/ + shared-memory.md
  → branch feature/TS-NNN-slug
  → 1 commit por task + atualizar shared-memory.md
  → 1 PR único → main
  → status Implemented
```

## Estrutura por feature

```
docs/techspecs/TS-NNN-slug/
├── planning.md
├── techspec.md
├── shared-memory.md
└── tasks/
    ├── TASK-01-*.md
    └── TASK-02-*.md
```

## Numeração

1. Liste pastas existentes em `docs/techspecs/`.
2. Próximo ID: `TS-001`, `TS-002`, … (três dígitos).
3. Slug em kebab-case: `TS-003-service-name-ingestao`.
4. Tasks: `TASK-01`, `TASK-02`, … dentro da pasta da techspec.

## Status da techspec

| Status | Significado |
|--------|-------------|
| `Draft` | Em elaboração |
| `In Review` | Aguardando revisão do usuário |
| `Approved` | Pode gerar tasks e iniciar implementação |
| `Implemented` | PR mergeado; feature entregue |

**Gates:**

- Tasks só após **`Approved`**
- PR só após **todas** as tasks `Done`
- ADR novo **antes** de `Approved`, quando houver decisão arquitetural

## Git workflow

| Artefato | Convenção |
|----------|-----------|
| Branch | `feature/TS-NNN-slug` |
| Commit | **1 por task** — `feat(TS-NNN): TASK-NN descrição` |
| PR | **1 por techspec** — após todas as tasks |

Exemplo de commit:

```
feat(TS-003): TASK-02 validar serviceName na ingestão

Implementa normalização lowercase e fallback default na API.
Ref: docs/techspecs/TS-003-service-name/tasks/TASK-02-api-validacao.md
```

## Shared memory

Arquivo [`shared-memory.md`](./TS-NNN-slug/shared-memory.md) por techspec:

- **Criado** em `hivelogs-task-breakdown`
- **Lido** antes de cada task (`hivelogs-task-implement`)
- **Atualizado** após cada task (decisões, contratos, gotchas)

Template: [docs/templates/shared-memory-template.md](../templates/shared-memory-template.md)

## Índice de techspecs

| ID | Título | Status | MVP | Branch |
|----|--------|--------|-----|--------|
| TS-001 | Fundação da Arquitetura Backend | Implemented | MVP 1 | `feature/TS-001-backend-architecture-foundation` |
| TS-002 | Core Domain — Organizations, Applications and Environments | Implemented | MVP 1 | `feature/TS-002-core-domain` |
| TS-003 | Self-hosted Setup and Access Model | Implemented | MVP 1 | `feature/TS-003-self-hosted-setup-access-model` |
| TS-004 | Web App Foundation | Implemented | MVP 1 | `feature/TS-004-web-app-foundation` |

> Atualize esta tabela ao criar ou concluir techspecs.

## Templates

| Fase | Template |
|------|----------|
| Planning | [feature-planning-template.md](../templates/feature-planning-template.md) |
| Techspec | [techspec-template.md](../templates/techspec-template.md) |
| Task | [task-template.md](../templates/task-template.md) |
| Shared memory | [shared-memory-template.md](../templates/shared-memory-template.md) |

## Exemplo de referência (estrutura apenas)

Para a decisão já documentada em ADR 002 (`serviceName` / `moduleName`), uma techspec futura poderia ser:

```
TS-001-service-module-telemetry/
├── planning.md
├── techspec.md
├── shared-memory.md
└── tasks/
    ├── TASK-01-contratos.md
    ├── TASK-02-api-ingestao.md
    ├── TASK-03-sdk-dotnet.md
    └── TASK-04-sdk-js.md
```

Ordem sugerida: contratos → API → SDKs (cada task atualiza `shared-memory.md` para a próxima).

## Skills relacionadas

| Skill | Etapa |
|-------|-------|
| `hivelogs-feature-workflow` | Orquestração do fluxo completo |
| `hivelogs-feature-planning` | Planning + dúvidas |
| `hivelogs-techspec` | Techspec + ADR se necessário |
| `hivelogs-task-breakdown` | Tasks + shared-memory inicial |
| `hivelogs-commit-workflow` | Task completa: Dev → Docs → Review → commit |
| `hivelogs-task-implement` | Implementação, commit, memória, PR |
| `hivelogs-agent-dev` / `hivelogs-agent-docs` / `hivelogs-agent-code-review` | Agentes especializados |
