---
name: hivelogs-feature-planning
description: >-
  Inicia planejamento de feature no HiveLogs: classifica MVP, levanta dúvidas de
  negócio e gera planning.md em docs/techspecs/TS-NNN/. Use quando o usuário
  traz feature nova ou pede planejamento antes da techspec.
disable-model-invocation: true
---

# HiveLogs — Feature Planning

## Pré-requisitos

1. Invocar `hivelogs-mvp-scope` — classificar MVP
2. Invocar `hivelogs-context` — boundaries do monorepo

## Passos

1. Liste pastas em `docs/techspecs/` e defina próximo ID (`TS-NNN`).
2. Crie `docs/techspecs/TS-NNN-slug/planning.md` a partir de [feature-planning-template.md](../../../docs/templates/feature-planning-template.md).
3. Preencha problema, contexto, restrições e módulos candidatos.
4. Liste **todas** as dúvidas de regra de negócio em **Perguntas abertas**.
5. **Pergunte ao usuário** cada dúvida — nunca assuma.
6. Registre respostas em **Decisões tomadas**.
7. Atualize índice em [docs/techspecs/README.md](../../../docs/techspecs/README.md).

## Gate para techspec

Só avançar para `hivelogs-techspec` quando:

- Seção **Perguntas abertas** estiver vazia (todas respondidas)
- `status` no planning = `ReadyForTechspec`

## Não fazer

- Gerar techspec ou tasks nesta etapa
- Implementar código
- Pular classificação MVP

## Próximo passo

Skill `hivelogs-techspec` com base no `planning.md` concluído.
