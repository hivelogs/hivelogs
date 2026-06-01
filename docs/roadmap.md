# Roadmap do HiveLogs

Este documento descreve o plano de evolução do projeto por fases. Itens marcados com checkbox serão implementados nas respectivas MVPs.

## MVP 1 - Foundation

Base do sistema: autenticação, modelo de dados, ingestão e dashboard inicial.

- [ ] Autenticação básica (JWT)
- [ ] Organizações
- [ ] Aplicações
- [ ] Ambientes
- [ ] Modelo de chaves (API Keys)
- [ ] Frontend Public Key
- [ ] Backend Secret Key
- [ ] MCP Access Token (estrutura; uso efetivo no MVP 3)
- [ ] Ingestão de eventos frontend
- [ ] Ingestão de logs backend
- [ ] Ingestão de erros frontend
- [ ] Ingestão de erros backend
- [ ] Ingestão de requests HTTP
- [ ] `serviceName` como metadado de telemetria
- [ ] `moduleName` como metadado opcional de telemetria
- [ ] Normalização obrigatória de `serviceName` e `moduleName` para lowercase (somente no backend)
- [ ] Filtragem de telemetria por `serviceName`
- [ ] Agrupamento de logs/erros/requests por `serviceName`
- [ ] Dashboard inicial
- [ ] Docker Compose
- [ ] PostgreSQL/TimescaleDB

## MVP 2 - SDKs and Runtime Visibility

SDKs oficiais e visibilidade em tempo real.

- [ ] SDK .NET (`Microsoft.Extensions.Logging`) — captura e envio leve; validações no backend
- [ ] SDK JS (browser) — bundle pequeno; validações no backend
- [ ] Usuários online
- [ ] Heartbeat de sessão
- [ ] Filtros avançados no dashboard
- [ ] Filtragem avançada por `serviceName` e `moduleName`
- [ ] Gráficos agregados por `serviceName`
- [ ] Distribuição de erros por `serviceName`
- [ ] Performance de requests por `serviceName` e `moduleName`
- [ ] Agregações simples
- [ ] Worker de agregação
- [ ] Worker de retenção
- [ ] Visualização de métricas por ambiente
- [ ] Visualização de erros por ambiente
- [ ] Visualização de requests por ambiente

## MVP 3 - MCP and AI Reports

Integração com IA e relatórios automatizados.

- [ ] MCP server (`apps/mcp`)
- [ ] Tools para leitura de métricas
- [ ] Tools para leitura de logs
- [ ] Tools para leitura de erros
- [ ] Tools para análise de requests
- [ ] Relatórios por IA
- [ ] Análise de anomalias
- [ ] Comparação entre ambientes
- [ ] Resumo diário/semanal

## Future

Funcionalidades planejadas além dos MVPs iniciais.

- [ ] SDK Node.js
- [ ] Integração OpenTelemetry
- [ ] Go collector/agent
- [ ] Distributed tracing
- [ ] Custom dashboards
- [ ] Alert rules
- [ ] Deploy tracking
- [ ] SaaS option
- [ ] Escopo opcional de API Key por `serviceName`
- [ ] Registro de serviços ou catálogo salvo de serviços, se necessário

---

Para detalhes de arquitetura, consulte [architecture.md](./architecture.md).
Para o modelo de segurança, consulte [security-model.md](./security-model.md).
