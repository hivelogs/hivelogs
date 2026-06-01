# ADR 002 - Service and Module as Telemetry Dimensions

## Status

Accepted

## Context

O HiveLogs precisa separar telemetria (logs, erros, requests, eventos e métricas) por origem técnica — por exemplo `api`, `web`, `worker`, `notification-service` ou `payment-service`. Essa separação é essencial para filtrar, agrupar e visualizar dados em aplicações com múltiplos componentes, workers ou microserviços.

Exigir que o usuário **registre manualmente cada Service** no dashboard antes de enviar dados aumentaria significativamente o atrito de onboarding no MVP 1, especialmente para projetos indie e aplicações pequenas que querem integrar rapidamente.

## Decision

`serviceName` e `moduleName` serão tratados como **metadados/dimensões de telemetria**, enviados pela configuração do SDK ou diretamente no payload de ingestão.

- **`serviceName`** representa a origem técnica principal da telemetria. Se ausente, o backend usa o fallback **`default`**.
- **`moduleName`** é opcional e representa uma subdivisão dentro do `serviceName`.
- O backend deve **normalizar `serviceName` e `moduleName` para lowercase** antes de persistir — regra **obrigatória**, aplicada **somente no backend**. SDKs enviam os valores como configurados; não há exigência de normalização no cliente.
- Regras de negócio e validações complexas de ingestão permanecem **centralizadas no backend**; SDKs devem permanecer leves (captura + envio).
- **Service e Module não são entidades de domínio no MVP 1.** Não há cadastro manual, tabela de serviços ou registro prévio exigido.
- **`Organization`**, **`Application`** e **`Environment`** permanecem as entidades configuradas principais do sistema.
- **API Keys permanecem vinculadas ao `Environment`.** A mesma Backend Secret Key pode ser usada por múltiplos serviços no mesmo ambiente; a separação ocorre via metadados no payload.
- O backend deve **armazenar**, **filtrar**, **agrupar** e **exibir** dados usando `serviceName` e `moduleName` quando fornecidos, para logs, events, metrics, sessions, requests e errors, quando aplicável.

Exemplos de configuração:

```csharp
builder.Services.AddHiveLogs(options =>
{
    options.ApiKey = "hvl_sec_prod_xxx";
    options.Endpoint = "https://hivelogs.local";
    options.ServiceName = "notification-service";
    options.ModuleName = "email-sender";
});
```

```ts
hivelog.init({
  publicKey: "hvl_pub_prod_xxx",
  endpoint: "https://hivelogs.local",
  serviceName: "web",
  moduleName: "checkout",
});
```

## Consequences

### Positivas

- Menor atrito de onboarding — integração imediata sem cadastro prévio de serviços.
- Menos configuração manual no dashboard.
- Suporte natural a monolitos, workers e microserviços com a mesma chave por ambiente.
- Flexibilidade para separar dados por origem técnica conforme a arquitetura evolui.
- A mesma Backend Secret Key pode atender múltiplos componentes no mesmo environment.
- Normalização obrigatória para lowercase no backend reduz grupos duplicados e simplifica filtros, sem impor lógica extra nos SDKs.
- SDKs permanecem leves: capturam e enviam telemetria; validações e regras de negócio ficam centralizadas no backend.

### Trade-offs

- `serviceName` é fornecido pelo cliente e deve ser validado (tamanho, formato, caracteres).
- Não há autorização em nível de serviço no MVP 1 — metadados não definem permissões.
- Nomes inconsistentes entre integrações podem gerar grupos duplicados na visualização (mitigado pela normalização obrigatória para lowercase; variantes de formato ainda exigem disciplina do cliente).
- Pode ser necessário um catálogo, sugestões ou normalização adicional em fases futuras.

## Alternatives Considered

### 1. Service como entidade registrada manualmente

Cada serviço seria criado no dashboard antes de aceitar telemetria. **Rejeitado para MVP 1** por aumentar atrito de onboarding e exigir passo extra de configuração para cada componente técnico.

### 2. API Key separada por Service

Cada serviço (`api`, `worker`, `notification-service`) teria sua própria chave. **Rejeitado para MVP 1** por multiplicar gestão de chaves e complicar deploys, especialmente em monolitos e workers que compartilham o mesmo environment.

### 3. `serviceName` como metadado livre de telemetria

`serviceName` e `moduleName` são enviados como metadados no payload ou SDK, sem entidade de domínio nem registro prévio. **Escolhido para MVP 1** — equilibra separação técnica com simplicidade de integração.

Escopo futuro (fora do MVP 1): escopo opcional de API Key por `serviceName` e possível catálogo ou registro de serviços salvos.
