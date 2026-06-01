# apps/mcp

## Propósito

Servidor MCP (Model Context Protocol) do HiveLogs. Expõe tools de leitura e análise para assistentes de IA (Cursor, Claude Desktop, etc.).

## Responsabilidades

- Tools de leitura: métricas, logs, erros, requests.
- Relatórios e análises por IA (fases futuras).
- Autenticação via MCP Access Token (escopo read-only).
- Comunicação com a API ou banco para consultas.

## Estrutura

```
mcp/
├── src/     # Código-fonte do servidor MCP (a inicializar)
└── tests/   # Testes automatizados
```

## O que NÃO deve conter

- Ingestão de dados (eventos, logs, erros).
- Interface web ou dashboard.
- Lógica de agregação pesada (delegar ao `apps/worker`).
- Backend Secret Key ou Frontend Public Key.
- Regras de negócio de domínio (delegar à API).
