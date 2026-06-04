---
id: TS-003
titulo: Self-hosted Setup and Access Model
mvp: MVP 1
modulos_candidatos: [apps/api, docs]
autor: HiveLogs
data: 2026-06-04
status: Implemented
---

# Planning — TS-003: Self-hosted Setup and Access Model

## Problema / objetivo de negócio

Instalações self-hosted do HiveLogs precisam ser configuradas na primeira execução sem cadastro público. O operador deve criar organização e admin inicial de forma controlada, com senha de setup via ambiente, preparando usuários, roles e `MustChangePassword` para autenticação JWT nas features seguintes.

## Contexto

A TS-002 entregou `Organization → MonitoredApplication → Environment` sem autenticação. Esta feature adiciona `User`, `OrganizationMember`, `SetupState`, endpoints `/setup/status` e `/setup/initialize`, e hash de senha — sem login/JWT.

## Restrições

- Roadmap: [docs/roadmap.md](../../roadmap.md)
- Segurança: [docs/security-model.md](../../security-model.md)
- ADRs: [003](../../adr/003-backend-clean-architecture-and-error-model.md), [004](../../adr/004-core-domain-monitored-application.md), [005](../../adr/005-self-hosted-setup-and-access-model.md)
- Sem MediatR, sem CQRS estrito, sem ASP.NET Identity completo

## Perguntas abertas

> **Gate:** esta seção deve estar vazia (todas respondidas) antes de gerar a techspec.

_(nenhuma — requisitos fornecidos na spec da feature)_

## Decisões tomadas

| # | Pergunta | Decisão | Data |
|---|----------|---------|------|
| 1 | Registro público? | Não; sem `POST /auth/register` | 2026-06-04 |
| 2 | Proteção do setup? | `HIVELOGS_SETUP_PASSWORD` / `Setup:Password` | 2026-06-04 |
| 3 | Admin define senha? | Sim; backend só valida e hasheia | 2026-06-04 |
| 4 | MustChangePassword no setup? | `false` para admin inicial | 2026-06-04 |
| 5 | Membership vs PrimaryOrganizationId? | `OrganizationMember` desde já | 2026-06-04 |
| 6 | Algoritmo de hash? | `PasswordHasher<T>` ASP.NET Core, sem Identity completo | 2026-06-04 |
| 7 | Setup password inválido — HTTP? | `401 Unauthorized` | 2026-06-04 |
| 8 | Reset único admin? | Fora de escopo; documentar como futuro | 2026-06-04 |

## Fora de escopo

- Login, JWT, refresh, `GET /auth/me`
- Proteção global de endpoints existentes
- Criação de usuários por admin, frontend, e-mail
- Reset do único admin, SSO, OAuth, API Keys, ingestão

## Riscos e dependências

| Risco / dependência | Impacto | Mitigação |
|---------------------|---------|-----------|
| Setup password fraca em produção | Acesso indevido ao setup | Documentar rotação antes do primeiro setup; desabilitar setup após `Configured` |
| Perda do único admin | Lockout | ADR/techspec futura (recovery) |
| Depende TS-002 | Organization já existe | Reutilizar `Organization` e repositório |

## Próximo passo

Techspec `Approved` → tasks + branch `feature/TS-003-self-hosted-setup-access-model`.
