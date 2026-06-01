# apps/web

## Propósito

Dashboard web do HiveLogs. Interface para visualização e gestão de dados de observabilidade.

## Responsabilidades

- Dashboard de métricas, sessões, eventos, logs, erros e requests.
- Gestão de organizações, aplicações, ambientes e chaves (fases futuras).
- Autenticação via JWT (token obtido da API).
- Stack: React, TypeScript, Vite, shadcn/ui, TanStack Query, React Hook Form, Zod.

## Estrutura

```
web/
├── src/     # Código-fonte React (a inicializar com npm create vite)
└── public/  # Assets estáticos
```

## O que NÃO deve conter

- Backend Secret Key ou MCP Access Token.
- Lógica de ingestão de dados.
- Endpoints de API (consumir `apps/api`, não duplicar).
- Processamento assíncrono ou agregações.
- Código dos SDKs de ingestão.
