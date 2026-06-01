# packages/sdk-dotnet

## Propósito

SDK oficial .NET do HiveLogs para integração em aplicações backend.

## Responsabilidades

- Provider para `Microsoft.Extensions.Logging`.
- Envio de logs estruturados para a API de ingestão.
- Captura de erros e exceções do backend.
- Registro de requests HTTP (middleware ou handler).
- Configuração via Backend Secret Key e URL da API.
- Configuração de **`ServiceName`** e **`ModuleName`** opcional, enviados como metadados em todos os payloads de ingestão.

## Princípios de design

- **Manter o SDK leve** — captura e envio com overhead mínimo no app host.
- **Não implementar regras de negócio nem validações complexas** — normalização, fallbacks, limites e demais regras ficam em `apps/api`.
- Preferir envio assíncrono/não bloqueante; evitar processamento pesado no hot path da aplicação.

## Estrutura

```
sdk-dotnet/
├── src/     # Código-fonte da biblioteca (a inicializar)
└── tests/   # Testes automatizados
```

## O que NÃO deve conter

- Código do dashboard ou da API.
- Frontend Public Key (é exclusiva do SDK JS).
- Lógica de autenticação de usuário (JWT).
- UI ou componentes visuais.
- Regras de retenção ou agregação de dados.
- Regras de negócio de ingestão (normalização, validação de metadados, fallbacks).
- Validações complexas de payload (responsabilidade de `apps/api`).
