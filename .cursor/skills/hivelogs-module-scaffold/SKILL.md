---
name: hivelogs-module-scaffold
description: >-
  Cria estrutura ou README de novo módulo no monorepo HiveLogs (apps ou packages),
  seguindo boundaries e template padrão. Use ao adicionar app, package ou documentar
  um módulo novo.
disable-model-invocation: true
---

# HiveLogs — Scaffold de módulo

## Antes de criar

1. Confirme que o módulo não duplica responsabilidade de um app/package existente.
2. Leia [docs/architecture.md](../../../docs/architecture.md) para o boundary correto.
3. Use skill `hivelogs-mvp-scope` se a feature não estiver clara no roadmap.

## Estrutura mínima

```
apps/nome/          ou    packages/nome/
├── src/
├── tests/          (omitir em shared-contracts se só schemas)
└── README.md
```

Adicione `.gitkeep` em pastas vazias se necessário para Git.

## README

Copie [docs/templates/module-readme-template.md](../../../docs/templates/module-readme-template.md) e preencha em **PT-BR**:

- Propósito (1 frase)
- Responsabilidades (bullets)
- O que NÃO deve conter (obrigatório — anti-patterns)

## Após criar

- Atualize [docs/architecture.md](../../../docs/architecture.md) se o módulo for novo na arquitetura.
- Considere ADR (`hivelogs-adr`) se a decisão for significativa.

## Naming .NET (quando aplicável)

Projetos: `HiveLogs.{App|Package}.{Layer}` — ex: `HiveLogs.Api.Domain`, `HiveLogs.Api.Application`.
