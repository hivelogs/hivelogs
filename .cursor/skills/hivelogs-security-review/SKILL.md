---
name: hivelogs-security-review
description: >-
  Revisa código e configuração do HiveLogs contra o modelo de segurança: API Keys,
  JWT, ingestão, variáveis de ambiente e SDKs. Use em auth, ingestão, env files,
  SDKs ou antes de merge de PRs sensíveis.
disable-model-invocation: true
---

# HiveLogs — Security Review

Fonte: [docs/security-model.md](../../../docs/security-model.md)

## Checklist — bloquear se falhar

- [ ] **Backend Secret Key** ausente de `apps/web`, bundles JS, `VITE_*`, repositório público
- [ ] **MCP Access Token** ausente de SDKs e código de ingestão browser
- [ ] **Frontend Public Key** sem escopos `read:*` ou admin
- [ ] JWT de usuário não usado em SDKs de ingestão
- [ ] Nenhum `.env` ou secret real commitado
- [ ] Ingestão browser validará `Origin` / allowed origins (quando implementado)

## Checklist — alertar

- [ ] Novo endpoint de ingestão sem schema em `packages/shared-contracts`
- [ ] Payload sem limite de tamanho documentado
- [ ] Chave com escopo maior que o necessário (viola menor privilégio)
- [ ] Dados de um Environment acessíveis de outro

## Credenciais — regras rápidas

| Credencial | Pode | Não pode |
|------------|------|----------|
| Frontend Public Key | Ingerir eventos/erros browser | Ler dados, logs backend, admin |
| Backend Secret Key | Ingerir logs/erros/requests backend | Front-end, leitura, admin |
| MCP Access Token | Ler métricas/logs/erros/requests | Ingerir, mutar config |
| JWT usuário | Dashboard, gestão | Ingestão via SDK |

## Saída esperada

Liste achados como: **Bloqueante** | **Alerta** | **OK**, com arquivo/linha quando possível.
