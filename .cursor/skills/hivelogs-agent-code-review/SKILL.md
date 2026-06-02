---
name: hivelogs-agent-code-review
description: >-
  Executa code review obrigatório antes de commit (agente HiveLogs Code Review).
  Bloqueia commit se veredito BLOQUEADO. Use imediatamente antes de git commit em tasks.
disable-model-invocation: true
---

# HiveLogs — Agent Code Review

Atua como **HiveLogs Code Review Agent**. Definição completa: [.cursor/agents/hivelogs-code-review.mdc](../../agents/hivelogs-code-review.mdc).

## Gate obrigatório

**Nenhum `git commit` de task** sem review **APROVADO** nesta sessão.

Se o usuário pedir commit direto, responder que o gate exige review primeiro e executar esta skill.

## Quando usar

- Imediatamente **antes** de `git commit` em uma task
- Após Dev + Docs concluírem
- Ao revisar PR (opcional: skill `requesting-code-review` externa)

## Procedimento

1. `git status` e `git diff` (staged + unstaged)
2. Ler task + shared-memory + critérios de aceite
3. Aplicar checklist do agente (arquitetura, segurança, escopo, testes)
4. Emitir veredito **APROVADO** ou **BLOQUEADO** com template do agente
5. Se BLOQUEADO → `hivelogs-agent-dev` para correções → repetir review
6. Se APROVADO → autorizar **um** commit conforme `hivelogs-task-implement`

## Invocação no Cursor

```
@HiveLogs Code Review revise TASK-NN antes do commit
```
