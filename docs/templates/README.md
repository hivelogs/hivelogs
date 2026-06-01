# Templates do HiveLogs

Templates para artefatos recorrentes de desenvolvimento. Copie o arquivo adequado, preencha os campos e salve no local indicado.

## Quando usar cada template

| Template | Destino | Skill relacionada |
|----------|---------|-------------------|
| [feature-planning-template.md](./feature-planning-template.md) | `docs/techspecs/TS-NNN/planning.md` | `hivelogs-feature-planning` |
| [techspec-template.md](./techspec-template.md) | `docs/techspecs/TS-NNN/techspec.md` | `hivelogs-techspec` |
| [task-template.md](./task-template.md) | `docs/techspecs/TS-NNN/tasks/TASK-NN.md` | `hivelogs-task-breakdown` |
| [shared-memory-template.md](./shared-memory-template.md) | `docs/techspecs/TS-NNN/shared-memory.md` | `hivelogs-task-breakdown`, `hivelogs-task-implement` |
| [adr-template.md](./adr-template.md) | `docs/adr/NNN-titulo.md` | `hivelogs-adr` |
| [issue-template.md](./issue-template.md) | Issue tracker (copiar corpo) | `hivelogs-mvp-scope` |
| [pr-description-template.md](./pr-description-template.md) | Descrição do PR (1 PR por techspec) | `hivelogs-task-implement` |
| [module-readme-template.md](./module-readme-template.md) | `apps/*/README.md` ou `packages/*/README.md` | `hivelogs-module-scaffold` |

## Numeração de techspecs

1. Liste pastas em `docs/techspecs/`.
2. Próximo ID: `TS-001`, `TS-002`, …
3. Slug em kebab-case no nome da pasta.

## Numeração de ADRs

1. Liste ADRs existentes em `docs/adr/`.
2. Use o próximo número sequencial com três dígitos: `002-`, `003-`, etc.
3. Título em kebab-case no nome do arquivo.

## Idioma

- ADRs e issues internas: **PT-BR**
- README raiz e documentação de usuário final: **inglês** (ver [README.md](../../README.md))
