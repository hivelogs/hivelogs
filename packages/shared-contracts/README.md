# packages/shared-contracts

## Propósito

Contratos e tipos compartilhados entre API, frontend, SDKs e documentação. Fonte única de verdade para formatos de dados.

## Responsabilidades

- Schemas de eventos e payloads de ingestão.
- Campos de dimensão de telemetria **`serviceName`** (obrigatório com fallback `default`) e **`moduleName`** (opcional) nos contratos aplicáveis. SDKs enviam os valores como configurados; o backend persiste em **lowercase** após normalização.
- Contratos descrevem formato e tipos — **não** implicam validação complexa no SDK; regras de negócio ficam em `apps/api`.
- Enums compartilhados (tipos de evento, severidade, ambientes).
- Tipos gerados a partir de OpenAPI (futuro).
- Documentação dos payloads aceitos pela API.
- Validação de formato (schemas JSON/Zod — a definir).

## Estrutura

```
shared-contracts/
└── src/     # Schemas, tipos e enums (a popular conforme API evoluir)
```

## Conteúdo previsto (fases futuras)

| Tipo | Exemplo |
|------|---------|
| Tipos OpenAPI | Gerados a partir do spec da API |
| Schemas de eventos | `EventPayload`, `LogPayload`, `ErrorPayload`, `RequestPayload` |
| Dimensões de telemetria | `serviceName`, `moduleName` em payloads de logs, erros, requests, eventos e métricas |
| Contratos de ingestão | Batch request/response |
| Enums | `EventType`, `LogLevel`, `EnvironmentName` |
| Documentação | Descrição de cada campo e limites |

## O que NÃO deve conter

- Regras de negócio ou lógica de validação server-side complexa.
- Código de infraestrutura (Docker, CI).
- Implementação de controllers ou endpoints.
- Secrets, API Keys ou configuração de runtime.
- Código específico de SDK (providers, middleware).
