---
name: hivelogs-context
description: >-
  Carrega boundaries do monorepo HiveLogs, hierarquia Organization/Application/Environment
  e anti-patterns por módulo. Use em qualquer tarefa no repositório HiveLogs antes de
  implementar ou refatorar código.
disable-model-invocation: true
---

# HiveLogs — Contexto do monorepo

Leia antes de implementar: [docs/architecture.md](../../../docs/architecture.md), [docs/security-model.md](../../../docs/security-model.md), [docs/roadmap.md](../../../docs/roadmap.md).

## Hierarquia de dados

```
Organization → Application → Environment
```

Todo dado e chave pertence a um **Environment**. Nunca misturar dados entre ambientes.

## Módulos — pode / não pode

| Módulo | Pode | Não pode |
|--------|------|----------|
| `apps/api` | Ingestão, JWT, API keys, persistência | UI, agregação pesada, tools MCP, código SDK |
| `apps/web` | Dashboard, gestão via JWT | Ingestão, Backend Secret, worker, retenção |
| `apps/worker` | Agregações, retenção, jobs | HTTP ingestão, UI, JWT de usuário |
| `apps/mcp` | Tools leitura, relatórios IA | Ingestão, UI, agregação pesada |
| `packages/sdk-js` | Eventos browser, erros JS, sessão | Backend Secret Key |
| `packages/sdk-dotnet` | Logs, erros backend, requests | Frontend Public Key, UI |
| `packages/shared-contracts` | Schemas, enums, tipos OpenAPI | Regras de negócio, infra |

## Chaves (resumo)

| Chave | Onde | Regra |
|-------|------|-------|
| Frontend Public Key | Browser / SDK JS | Pública; só ingestão browser |
| Backend Secret Key | Servidor / SDK .NET | Secreta; nunca no front |
| MCP Access Token | `apps/mcp` | Secreta; preferir read-only |

## Contratos

Novos payloads de ingestão → definir em `packages/shared-contracts` antes da implementação na API.

## Idioma

- Dev/docs/ADR/skills: **PT-BR**
- README raiz e docs de usuário: **inglês**

## Dúvidas de negócio

Se regra de negócio for ambígua, **pergunte ao usuário** antes de assumir.
