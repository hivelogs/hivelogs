---
name: hivelogs-task-breakdown
description: >-
  Quebra techspec Approved em tasks autocontidas em Markdown, inicializa
  shared-memory.md e atualiza índice. Use após techspec com status Approved.
disable-model-invocation: true
---

# HiveLogs — Task Breakdown

## Pré-requisitos

- `techspec.md` com status **`Approved`**
- Skill `hivelogs-techspec` concluída

## Passos

1. Validar status `Approved` em `techspec.md` — abortar se diferente.
2. Quebrar trabalho por **módulo + dependência**; uma task = um módulo principal.
3. Gerar `docs/techspecs/TS-NNN-slug/tasks/TASK-NN-slug.md` via [task-template.md](../../../docs/templates/task-template.md).
4. Garantir **autocontenção**: contexto mínimo, IN/OUT, aceite, verificação em cada task.
5. Definir `depends_on` entre tasks (ex.: `shared-contracts` antes de `api`).
6. Inicializar `shared-memory.md` via [shared-memory-template.md](../../../docs/templates/shared-memory-template.md).
7. Atualizar índice em [docs/techspecs/README.md](../../../docs/techspecs/README.md).

## Regras de quebra

- Task pequena o suficiente para **1 commit**
- Escopo OUT explícito em cada task (anti scope creep)
- Se task precisar de info de outra → `depends_on` + leitura de `shared-memory.md`
- SDKs leves; validação no backend quando tocar ingestão

## Não fazer

- Implementar código
- Gerar tasks se techspec não estiver `Approved`
- Tasks que exijam ler toda a techspec para entender o básico

## Próximo passo

1. Criar branch `feature/TS-NNN-slug`
2. Skill `hivelogs-task-implement` para cada task na ordem de `depends_on`
