## Summary

Descreva em 1–3 frases o que mudou e por quê.

## Techspec

**ID:** TS-NNN  
**Branch:** feature/TS-NNN-slug  
**Tasks incluídas:** TASK-01, TASK-02, …

## Módulos afetados

- [ ] apps/api
- [ ] apps/web
- [ ] apps/worker
- [ ] apps/mcp
- [ ] packages/sdk-dotnet
- [ ] packages/sdk-js
- [ ] packages/shared-contracts
- [ ] infra
- [ ] docs

## MVP

MVP 1 | MVP 2 | MVP 3 | N/A (infra/docs only)

## Test plan

- [ ] 
- [ ] Comandos executados: ` `

## Security checklist

- [ ] Nenhuma Backend Secret Key ou MCP Access Token no front-end ou em `VITE_*`
- [ ] Frontend Public Key não recebeu escopos de leitura ou admin
- [ ] Novos payloads de ingestão documentados em `shared-contracts` (se aplicável)
- [ ] Sem secrets ou `.env` commitados

## Docs

- [ ] ADR criado/atualizado (se decisão arquitetural)
- [ ] Techspec status → `Implemented` (se PR de feature)
- [ ] `shared-memory.md` reflete estado final da implementação
- [ ] README do módulo atualizado (se boundaries mudaram)
- [ ] `docs/architecture.md` ou `security-model.md` (se aplicável)
