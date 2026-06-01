# Modelo de Segurança do HiveLogs

## Visão geral

O HiveLogs utiliza **dois planos de autenticação** distintos:

| Plano | Mecanismo | Uso |
|-------|-----------|-----|
| Usuário (dashboard) | JWT | Login, gestão de orgs/apps/envs, visualização de dados |
| Ingestão e integração | API Keys / Tokens | SDKs, servidores backend, servidor MCP |

Esses planos não devem ser misturados: uma Frontend Public Key não substitui um JWT de usuário, e um JWT de usuário não deve ser embutido em SDKs de ingestão.

## Tipos de credenciais

### Frontend Public Key

**Natureza:** pública por design. Pode e será exposta no bundle JavaScript do cliente.

**Uso:**

- SDK JS no browser.
- Ingestão de eventos, page views, erros JS e heartbeat de sessão.

**Escopos esperados (futuro):**

- `ingest:events`
- `ingest:errors:frontend`
- `ingest:sessions`

**Restrições:**

- Associada a um único Environment.
- Validada contra **allowed origins** (lista de domínios permitidos configurada no ambiente).
- Rate limiting por chave e por IP.
- Não permite ingestão de logs de backend nem leitura de dados.

**Regra fundamental:**

> A Frontend Public Key pode ser exposta no navegador e deve ser tratada como pública por natureza. Nunca conceda a ela permissões de leitura ou operações administrativas.

---

### Backend Secret Key

**Natureza:** secreta. Equivalente a uma senha de API server-side.

**Uso:**

- SDK .NET e integrações server-side.
- Ingestão de logs, erros de backend, requests HTTP e métricas server-side.

**Escopos esperados (futuro):**

- `ingest:logs`
- `ingest:errors:backend`
- `ingest:requests`
- `ingest:metrics`

**Restrições:**

- Associada a um único Environment.
- Rate limiting por chave.
- Validação rigorosa de payload (tamanho, schema, tipos).

**Regra fundamental:**

> A Backend Secret Key nunca deve ser exposta no front-end, em repositórios públicos, em variáveis `VITE_*` ou em bundles JavaScript.

---

### MCP Access Token

**Natureza:** secreta, com escopo limitado e preferencialmente read-only.

**Uso:**

- Servidor MCP (`apps/mcp`) para consultas e análises por IA.
- Tools de leitura: métricas, logs, erros, requests.

**Escopos esperados (futuro):**

- `read:metrics`
- `read:logs`
- `read:errors`
- `read:requests`
- `read:reports` (fase MCP)

**Restrições:**

- Associado a Organization ou Application (a definir na implementação).
- Revogável independentemente das chaves de ingestão.
- Sem permissão de ingestão ou mutação de dados.
- Rate limiting específico para queries MCP.

**Regra fundamental:**

> O MCP Access Token deve ser escopado, preferencialmente read-only, e revogável a qualquer momento sem impactar a ingestão.

---

## API Keys e Environment

As API Keys permanecem vinculadas a um **Environment**. A separação por serviço não exige chaves distintas:

- A mesma **Backend Secret Key** pode ser usada por múltiplos serviços no mesmo ambiente (ex.: `api`, `worker`, `notification-service`).
- A separação por serviço ocorre via **`serviceName`** (e opcionalmente **`moduleName`**) no payload de ingestão.
- A **Frontend Public Key** também pode enviar `serviceName`, geralmente `web`.
- O backend deve validar o **tipo de chave** e o **tipo de ingestão**, mas **não** deve exigir registro prévio de `serviceName`.
- No futuro, API Keys poderão ser opcionalmente escopadas por `serviceName`; isso **não faz parte do MVP 1**.

---

## Service and Module Metadata

`serviceName` e `moduleName` são enviados pelo cliente/SDK e devem ser tratados como **metadados não confiáveis**.

### Implicações de segurança

- Não devem ser usados como dados de **autorização** no MVP 1.
- Devem passar por validação de tamanho, formato e caracteres permitidos.
- Devem ser **normalizados para lowercase exclusivamente pelo backend** antes da persistência (obrigatório). SDKs enviam o valor configurado; a transformação não é responsabilidade do cliente.
- Não devem conter dados sensíveis (PII, secrets, tokens).
- Devem ser usados apenas para **filtragem**, **agrupamento**, **visualização** e **relatórios**.

### Regras de validação sugeridas

| Campo | Regra |
|-------|-------|
| `serviceName` | Obrigatório após normalização; fallback `default` quando ausente |
| `moduleName` | Opcional |
| Tamanho máximo | 100 caracteres (recomendado) |
| Caracteres permitidos | Letras, números, hífen (`-`), underscore (`_`) e ponto (`.`) |
| Normalização | **Obrigatória no backend:** converte `serviceName` e `moduleName` para lowercase antes de persistir; SDKs não aplicam essa regra |

---

## Isolamento por ambiente

Todo dado ingerido ou consultado está vinculado a:

```
Organization → Application → Environment
```

- Chaves de ingestão pertencem a um **Environment** específico.
- Dados de `staging` nunca se misturam com `production`.
- Consultas do dashboard e do MCP respeitam o escopo do token/chave.

## Allowed origins (Frontend Public Key)

Para mitigar uso indevido de chaves públicas vazadas:

1. Cada Environment define uma lista de **allowed origins** (ex: `https://app.example.com`).
2. A API valida o header `Origin` ou `Referer` nas requisições de ingestão browser.
3. Requisições de origens não listadas são rejeitadas (HTTP 403).

> Origins devem ser configuráveis pelo administrador do ambiente, sem necessidade de redeploy do SDK.

## Rate limiting

Aplicado em camadas:

| Camada | Alvo |
|--------|------|
| Por API Key | Limite de requisições/minuto por chave |
| Por IP | Proteção contra abuso mesmo com chave válida |
| Por Organization | Teto global opcional por plano (futuro SaaS) |
| MCP | Limite de queries por token |

Comportamento esperado: HTTP 429 com header `Retry-After`.

## Validação de payload

Todas as requisições de ingestão são **validadas no backend** (`apps/api`). SDKs enviam payloads sem executar regras de negócio ou validações complexas — isso mantém os clientes leves e evita impacto no app host.

A API deve:

1. Exigir `Content-Type: application/json`.
2. Validar contra schemas definidos em `packages/shared-contracts`.
3. Aplicar regras de negócio de ingestão (normalização, fallbacks, limites de tamanho e caracteres).
4. Respeitar tamanho máximo por batch (a definir; sugestão inicial: 1 MB).
5. Rejeitar campos desconhecidos críticos ou tipos inválidos (HTTP 400).

Dados sensíveis (PII) devem ser evitados nos payloads; documentação futura abordará sanitização recomendada.

## Rotação e revogação de chaves

- Cada Environment pode ter **múltiplas chaves ativas** do mesmo tipo (para rotação sem downtime).
- Revogação é **imediata**: chave revogada retorna HTTP 401.
- Rotação recomendada:
  1. Gerar nova chave.
  2. Atualizar deploy/SDK.
  3. Revogar chave antiga após período de transição.
- Revogação de MCP Access Token não afeta ingestão.
- Revogação de Backend Secret Key não afeta Frontend Public Key.

## Princípio de menor privilégio

| Credencial | Pode | Não pode |
|------------|------|----------|
| Frontend Public Key | Ingerir eventos/erros browser | Ler dados, ingerir logs backend, admin |
| Backend Secret Key | Ingerir logs/erros/requests backend | Ler dados de outros ambientes, admin |
| MCP Access Token | Ler métricas/logs/erros/requests | Ingerir dados, mutar configuração |
| JWT de usuário | Gestão e leitura no dashboard | Ingerir dados via SDK |

## JWT (usuários do dashboard)

Autenticação separada das API Keys:

- Emitido após login (email/senha ou OAuth — a definir).
- Contém claims: `user_id`, `organization_id`, roles.
- Expiração curta com refresh token (a definir na implementação).
- Usado exclusivamente por `apps/web` → `apps/api`.
- Nunca enviado em SDKs de ingestão.

## Referências

- [architecture.md](./architecture.md) — visão dos módulos e conceitos
- [adr/002-service-and-module-as-telemetry-dimensions.md](./adr/002-service-and-module-as-telemetry-dimensions.md) — ADR: service e module como dimensões de telemetria
- [roadmap.md](./roadmap.md) — quando cada controle será implementado
