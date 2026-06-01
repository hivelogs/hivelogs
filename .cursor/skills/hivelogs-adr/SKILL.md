---
name: hivelogs-adr
description: >-
  Cria ou revisa Architecture Decision Records do HiveLogs no formato padrão,
  com numeração sequencial em docs/adr/. Use quando houver decisão arquitetural,
  mudança de stack, ou o usuário pedir um ADR.
disable-model-invocation: true
---

# HiveLogs — ADR

## Passos

1. Liste arquivos em `docs/adr/` e determine o próximo número (`001`, `002`, …).
2. Copie [docs/templates/adr-template.md](../../../docs/templates/adr-template.md).
3. Salve como `docs/adr/NNN-titulo-em-kebab-case.md`.
4. Preencha todas as seções em **PT-BR**.
5. Status inicial: `Proposed` (mudar para `Accepted` após revisão).

## Formato obrigatório

- Status
- Context
- Decision
- Consequences (positivas e negativas)
- Alternatives Considered (mínimo uma alternativa rejeitada)

## Referência

ADR existente: [docs/adr/001-monorepo-and-stack.md](../../../docs/adr/001-monorepo-and-stack.md)

## Não fazer

- ADRs fora de `docs/adr/`
- Decisões de implementação trivial (preferir comentário no PR)
- Duplicar conteúdo inteiro de `architecture.md` (referencie o doc)
