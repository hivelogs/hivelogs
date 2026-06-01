# apps/worker

## Propósito

Serviço de processamento assíncrono do HiveLogs. Executa jobs em background que não devem bloquear a API de ingestão.

## Responsabilidades

- Agregações temporais (rollups, contadores, percentis).
- Políticas de retenção e limpeza de dados antigos.
- Processamento pós-ingestão (filas, eventos — a definir).
- Serviço .NET separado, deployável de forma independente.

## Estrutura

```
worker/
├── src/     # Código-fonte (a inicializar com dotnet new worker)
└── tests/   # Testes automatizados
```

## O que NÃO deve conter

- Endpoints HTTP públicos de ingestão.
- Interface de usuário.
- Autenticação de usuário final (JWT).
- Tools MCP.
- Código dos SDKs.
