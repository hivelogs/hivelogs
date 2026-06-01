# apps/api

## Propósito

API HTTP principal do HiveLogs. Responsável por ingestão de dados, autenticação de usuários e gestão de recursos do sistema.

## Responsabilidades

- Endpoints de ingestão (eventos, logs, erros, requests HTTP).
- Regras de negócio e validações de ingestão no servidor (schema, metadados, normalização, limites, fallbacks).
- Recebimento, validação, **normalização obrigatória para lowercase (exclusiva do backend)** e persistência de `serviceName` e `moduleName` nos payloads de ingestão.
- Autenticação JWT para o dashboard.
- Validação de API Keys (Frontend Public Key, Backend Secret Key).
- Gestão de organizações, aplicações e ambientes.
- Persistência em PostgreSQL/TimescaleDB.
- Clean Architecture (.NET).

## Estrutura

```
api/
├── src/     # Código-fonte (a inicializar com dotnet new)
└── tests/   # Testes automatizados
```

## O que NÃO deve conter

- Interface de usuário (HTML, React, assets estáticos).
- Lógica de agregação pesada ou jobs em background (use `apps/worker`).
- Implementação de tools MCP (use `apps/mcp`).
- Código dos SDKs (use `packages/sdk-*`).
- Secrets hardcoded ou arquivos `.env` commitados.
