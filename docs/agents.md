# Agentes de código — HiveLogs

O repositório define **três agentes especializados** versionados em [`.cursor/agents/`](../.cursor/agents/). Eles dividem desenvolvimento, documentação e revisão — com **code review obrigatório antes de cada commit** de task.

## Por que três agentes?

| Problema | Solução |
|----------|---------|
| Mesmo agente codando e commitando sem revisão | Dev Agent não commita |
| Docs desatualizada após código | Docs Agent dedicado |
| Violações de arquitetura/segurança no commit | Code Review Agent como gate |

## Agentes

### HiveLogs Dev

- **Arquivo:** [`.cursor/agents/hivelogs-dev.mdc`](../.cursor/agents/hivelogs-dev.mdc)
- **Skill:** `hivelogs-agent-dev`
- **Faz:** código, testes, `dotnet build` / `dotnet test`
- **Não faz:** commit, planning/techspec/ADR

### HiveLogs Docs

- **Arquivo:** [`.cursor/agents/hivelogs-docs.mdc`](../.cursor/agents/hivelogs-docs.mdc)
- **Skill:** `hivelogs-agent-docs`
- **Faz:** `shared-memory.md`, tasks, README de módulo, ADRs, roadmap
- **Não faz:** commit, código de produção

### HiveLogs Code Review

- **Arquivo:** [`.cursor/agents/hivelogs-code-review.mdc`](../.cursor/agents/hivelogs-code-review.mdc)
- **Skill:** `hivelogs-agent-code-review`
- **Faz:** revisa diff vs techspec, ADR 003, segurança, escopo IN
- **Não faz:** commit se veredito **BLOQUEADO**

## Fluxo por task (obrigatório)

```
Dev → Docs → Code Review → (se APROVADO) git commit
                ↑
                └── se BLOQUEADO → Dev (correções) → Docs → Code Review
```

Orquestração: skill **`hivelogs-commit-workflow`**.

### No Cursor

1. `@HiveLogs Dev` — implementar TASK-NN
2. `@HiveLogs Docs` — atualizar documentação da task
3. `@HiveLogs Code Review` — revisar antes do commit
4. Commit **somente** após veredito **APROVADO**

Ou um único pedido:

> Execute `hivelogs-commit-workflow` para TASK-NN da TS-002

## Integração com o fluxo de feature

| Fase | Agente típico |
|------|----------------|
| Planning / techspec / tasks | Docs |
| Implementação TASK-NN | Dev → Docs → Code Review → commit |
| PR único (todas tasks Done) | Review humano + opcional Code Review no diff total |

Detalhes: [docs/techspecs/README.md](./techspecs/README.md), skill `hivelogs-feature-workflow`.

## O que não substitui

- **Regras** em [`.cursor/rules/`](../.cursor/rules/) — contexto automático por arquivo
- **Skills** de domínio (`hivelogs-security-review`, `hivelogs-context`, etc.)
- Revisão humana no PR

## Referências

- [AGENTS.md](../AGENTS.md) — índice mestre
- [.cursor/agents/README.md](../.cursor/agents/README.md) — índice dos agentes
