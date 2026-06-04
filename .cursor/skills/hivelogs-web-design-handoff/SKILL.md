---
name: hivelogs-web-design-handoff
description: >-
  Converte protótipo Pencil.dev e docs/design.md em tela React do HiveLogs:
  leitura via MCP Pencil, gaps de negócio, layout full-screen e checklist visual.
  Use ao implementar UI nova, setup/onboarding ou alinhar front ao design system.
disable-model-invocation: true
---

# HiveLogs — Design Handoff (Pencil → React)

Guia completo: [docs/guides/design-handoff-pencil.md](../../../docs/guides/design-handoff-pencil.md)

## Artefatos

- [docs/design.md](../../../docs/design.md) — design system
- [docs/ui/pencil.pen](../../../docs/ui/pencil.pen) — protótipo
- [docs/Logo.svg](../../../docs/Logo.svg) — copiar para `apps/web/src/assets/`

## Fluxo

1. `get_editor_state(include_schema: true)` se schema Pencil ausente
2. `batch_get` no frame da tela (ex. buscar `name: "Initial Setup Screen"`)
3. Listar labels, CTA, seções, cores — registrar em `shared-memory.md`
4. Comparar com techspec/backend — **campos obrigatórios ausentes no protótipo**
5. Implementar em React (Tailwind + shadcn); validar com `get_screenshot` no final

## Regras de implementação

- Dark-first; tokens em `globals.css`
- Onboarding/setup: layout dedicado, sem sidebar/header do app logado
- Microcopy: inglês até decisão de produto (TS-005)
- CTA: preferir texto do protótipo (“Create instance” vs genérico)
- Não copiar link/navegação que conflita com gates de rota (ex. “Back to sign in” durante setup pendente)

## Pencil — não fazer

- Ler `.pen` com Read/Grep do workspace
- Screenshots em excesso (custo de contexto)
- `readDepth` > 3 em `batch_get` sem necessidade

## Registrar em shared-memory

- Frame id Pencil usado
- Gaps protótipo × backend
- Decisões de copy/layout não óbvias no `.pen`
