---
name: hivelogs-task-implement
description: >-
  Implementa uma task de techspec: lê shared-memory, escopo IN only, 1 commit
  por task, atualiza shared-memory e abre PR único ao completar todas as tasks.
  Use ao desenvolver TASK-NN de docs/techspecs/.
disable-model-invocation: true
---

# HiveLogs — Task Implement

## Pré-requisitos

- Tasks geradas em `docs/techspecs/TS-NNN-slug/tasks/`
- `shared-memory.md` inicializado
- Branch `feature/TS-NNN-slug` criada a partir de `main`

## Antes de codar

1. Ler a task (`TASK-NN-*.md`)
2. Ler **`../shared-memory.md`**
3. Confirmar tasks em `depends_on` com `status: Done`
4. Confirmar escopo **IN** — ignorar escopo **OUT**

## Implementação

1. Implementar **apenas** o escopo IN da task
2. Respeitar boundaries (`hivelogs-context`)
3. Segurança se aplicável (`hivelogs-security-review`)
4. Executar verificação da task (comandos no markdown)
5. Marcar critérios de aceite

## Commit (obrigatório: 1 por task)

```
feat(TS-NNN): TASK-NN título curto

Descrição objetiva.
Ref: docs/techspecs/TS-NNN-slug/tasks/TASK-NN-slug.md
```

Incluir no **mesmo commit** quando possível:

- Código da task
- Atualização de `shared-memory.md`
- Task markdown com `status: Done`

## Atualizar shared-memory.md

Após cada task, append em seções relevantes:

- Decisões de implementação
- Contratos / APIs / schema alterados
- Gotchas para tasks seguintes
- Links (ADR criado, arquivos-chave)

Formato: `### [TASK-NN] título — YYYY-MM-DD`

Se decisão arquitetural nova surgir → `hivelogs-adr` + referência na shared-memory.

## Após a task

- [ ] `status: Done` na task
- [ ] `shared-memory.md` atualizado
- [ ] `tasks_concluidas` no frontmatter da shared-memory
- [ ] Commit único feito

## Última task → PR único

Quando **todas** as tasks estiverem `Done`:

1. Abrir **um PR** `feature/TS-NNN-slug` → `main`
2. Usar [pr-description-template.md](../../../docs/templates/pr-description-template.md)
3. Referenciar techspec e listar tasks/commits
4. Após merge → techspec status **`Implemented`**

## Não fazer

- Múltiplos commits para a mesma task (salvo correção solicitada)
- PR antes de todas as tasks concluídas
- Push direto em `main`
- Implementar escopo OUT ou de outra task
