# packages/sdk-js

## Propósito

SDK oficial JavaScript do HiveLogs para integração em aplicações browser (SPA, SSR client-side).

## Responsabilidades

- Captura de eventos customizados de produto/analytics.
- Page views e navegação.
- Captura de erros JavaScript (window.onerror, unhandledrejection).
- Heartbeat de sessão e usuários online (fases futuras).
- Configuração via Frontend Public Key e URL da API.
- Configuração de **`serviceName`** (geralmente `web`) e **`moduleName`** opcional (ex.: `checkout`, `auth`, `catalog`), enviados como metadados em todos os payloads de ingestão.

## Princípios de design

- **Manter o bundle pequeno e o runtime leve** — impacto mínimo no browser e na aplicação host.
- **Não implementar regras de negócio nem validações complexas** — normalização, fallbacks, limites e demais regras ficam em `apps/api`.
- Preferir envio assíncrono/não bloqueante; evitar trabalho síncrono pesado no main thread.

## Estrutura

```
sdk-js/
├── src/     # Código-fonte da biblioteca (a inicializar)
└── tests/   # Testes automatizados
```

## O que NÃO deve conter

- Backend Secret Key (nunca no browser).
- Código server-side ou SDK Node.js (fora do escopo inicial).
- Dashboard ou componentes de UI do HiveLogs.
- Lógica de autenticação de usuário (JWT).
- Dependências pesadas desnecessárias (manter bundle pequeno).
- Regras de negócio de ingestão (normalização, validação de metadados, fallbacks).
- Validações complexas de payload (responsabilidade de `apps/api`).
