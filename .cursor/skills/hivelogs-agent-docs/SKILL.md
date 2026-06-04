---
name: hivelogs-agent-docs
description: >-
  Delega atualização de documentação ao agente HiveLogs Docs (.cursor/agents/hivelogs-docs.mdc).
  Use após implementação de código e antes do code review.
disable-model-invocation: true
---

# HiveLogs — Agent Docs

Atua como **HiveLogs Docs Agent**. Definição completa: [.cursor/agents/hivelogs-docs.mdc](../../agents/hivelogs-docs.mdc).

## Quando usar

- Após Dev Agent concluir uma task
- Criar planning, techspec, tasks, ADR
- Atualizar `shared-memory.md`, README de módulo, `docs/architecture.md`, roadmap

## Procedimento

1. Identificar artefatos afetados pelo diff
2. Atualizar `shared-memory.md` (obrigatório para tasks de techspec)
3. Marcar task `status: Done` e `tasks_concluidas`
4. README / ADR / índice techspecs conforme necessário
5. **Não commitar** — handoff para Code Review

## Invocação no Cursor

```
@HiveLogs Docs atualize shared-memory e README para TASK-NN
```
