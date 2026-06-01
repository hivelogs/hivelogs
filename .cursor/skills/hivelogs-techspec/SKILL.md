---
name: hivelogs-techspec
description: >-
  Gera techspec.md por módulo a partir do planning aprovado, referencia ou cria
  ADRs quando necessário, e gerencia status Draft/In Review/Approved. Use após
  planning sem dúvidas abertas.
disable-model-invocation: true
---

# HiveLogs — Techspec

## Pré-requisitos

- `docs/techspecs/TS-NNN-slug/planning.md` com perguntas abertas vazias
- `hivelogs-feature-planning` concluído

## Passos

1. Ler `planning.md` da techspec.
2. Gerar `techspec.md` a partir de [techspec-template.md](../../../docs/templates/techspec-template.md).
3. Detalhar **por módulo** (`apps/*`, `packages/*`, `infra`, `docs`): fazer, não fazer, arquivos previstos.
4. Definir ordem de implementação e rascunho de tasks.
5. Definir branch: `feature/TS-NNN-slug`.

## ADRs — quando criar

| Situação | Ação |
|----------|------|
| Decisão arquitetural nova (stack, boundary, modelo, segurança) | Invocar `hivelogs-adr` **antes** de `Approved` |
| Segue ADR existente | Referenciar em `adrs[]` no frontmatter |
| Implementação trivial | Registrar "nenhum ADR necessário" |

ADR **não** substitui techspec.

## Status

1. `Draft` — elaboração inicial
2. `In Review` — aguardando revisão do usuário
3. **`Approved`** — usuário aprova explicitamente; pode gerar tasks
4. `Implemented` — após merge do PR (não marcar manualmente antes)

Registrar transições em **Histórico de status** no techspec.

## Gate para tasks

**Não** invocar `hivelogs-task-breakdown` enquanto status ≠ **`Approved`**.

## Não fazer

- Gerar tasks ou código nesta etapa
- Marcar `Approved` sem confirmação do usuário
- Criar ADR para decisão trivial de implementação

## Próximo passo

Após `Approved` → skill `hivelogs-task-breakdown`.
