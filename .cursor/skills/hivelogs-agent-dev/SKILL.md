---
name: hivelogs-agent-dev
description: >-
  Delega implementação de código ao agente HiveLogs Dev (.cursor/agents/hivelogs-dev.mdc).
  Use ao desenvolver uma task de techspec ou corrigir achados bloqueantes do code review.
disable-model-invocation: true
---

# HiveLogs — Agent Dev

Atua como **HiveLogs Dev Agent**. Definição completa: [.cursor/agents/hivelogs-dev.mdc](../../agents/hivelogs-dev.mdc).

## Quando usar

- Implementar escopo IN de `docs/techspecs/TS-NNN/tasks/TASK-NN-*.md`
- Corrigir itens **Bloqueante** após code review
- Escrever ou ajustar testes da camada afetada

## Procedimento

1. Ler task + `shared-memory.md` + `hivelogs-context`
2. Implementar código e testes
3. Executar verificação da task (`dotnet build`, `dotnet test`, etc.)
4. **Não commitar** — handoff para Docs (se necessário) e Code Review

## Invocação no Cursor

```
@HiveLogs Dev implemente TASK-NN conforme docs/techspecs/TS-NNN/...
```

Ou peça ao orquestrador: `hivelogs-commit-workflow` para o fluxo completo.
