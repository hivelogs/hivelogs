# ADR 001 - Monorepo e Stack Inicial

## Status

Accepted

## Context

O HiveLogs precisa integrar múltiplos componentes com responsabilidades distintas: API de ingestão e gestão, dashboard web, processamento assíncrono (worker), servidor MCP para análises por IA, SDKs para clientes (.NET e JavaScript) e contratos compartilhados entre todos eles.

Projetos de observabilidade tendem a crescer rapidamente em superfície de integração. Separar repositórios desde o início aumentaria o custo de sincronizar contratos, versionar payloads de ingestão e manter consistência entre API, frontend e SDKs.

A equipe precisa de uma base que permita desenvolvimento local simples (Docker Compose), deploy independente por serviço e evolução incremental sem reestruturação prematura.

## Decision

Adotar um **monorepo** com a seguinte organização e stack:

| Área | Decisão |
|------|---------|
| Estrutura | Monorepo com `apps/`, `packages/`, `infra/` e `docs/` |
| Backend principal | .NET (versão estável mais recente), Clean Architecture |
| Banco de dados | PostgreSQL com extensão TimescaleDB |
| Frontend | React, TypeScript, Vite |
| Processamento assíncrono | `apps/worker` como serviço .NET separado |
| Integração com IA | `apps/mcp` como servidor MCP separado |
| SDKs | `packages/sdk-dotnet` e `packages/sdk-js` como pacotes independentes |
| Contratos | `packages/shared-contracts` para schemas, enums e tipos compartilhados |
| Desenvolvimento local | Docker Compose em `infra/` com TimescaleDB e placeholders para apps |

Os serviços de aplicação (`api`, `web`, `worker`, `mcp`) permanecem deployáveis de forma independente, mesmo dentro do mesmo repositório.

## Consequences

### Positivas

- Contratos de ingestão e tipos compartilhados evoluem em um único lugar.
- Refatorações que cruzam API, web e SDKs são mais simples de coordenar.
- Onboarding de contribuidores com visão única do sistema.
- Docker Compose unifica o ambiente local.

### Negativas

- Repositório maior; necessidade de disciplina em boundaries entre módulos.
- CI/CD futuro precisará de detecção de mudanças por path (a configurar em fase posterior).
- Curva de aprendizado do monorepo para contribuidores acostumados com multi-repo.

## Alternatives Considered

### Multi-repo (um repositório por app/package)

**Rejeitado** para a fase inicial: overhead alto de versionamento de contratos, releases coordenados e documentação fragmentada.

### Node.js no backend principal

**Rejeitado**: .NET oferece melhor alinhamento com o SDK .NET (`Microsoft.Extensions.Logging`), performance previsível para API de ingestão e ecossistema maduro para workers em background.

### Monólito único (API + worker + MCP no mesmo processo)

**Rejeitado**: worker e MCP têm perfis de carga e ciclo de deploy diferentes da API de ingestão; separação permite escalar e reiniciar componentes de forma independente.

### PostgreSQL sem TimescaleDB

**Adiado como opção**: TimescaleDB é escolhido desde o início por otimização nativa para séries temporais (métricas, eventos, logs), comuns em plataformas de observabilidade.
