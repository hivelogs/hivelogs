# ADR 004 - Core Domain Model e MonitoredApplication

## Status

Accepted

## Context

A hierarquia **Organization → Application → Environment** é o núcleo de isolamento do HiveLogs. A Feature 002 implementa essas entidades no backend.

O termo **Application** é ambíguo no ecossistema .NET e neste monorepo:

- `System.Application` e tipos do runtime
- Projeto/camada `HiveLogs.Application` (casos de uso)
- Conceito de domínio: uma aplicação monitorada dentro de uma organização

Usar `Application` como nome de entidade no `HiveLogs.Domain` aumenta risco de conflitos de namespace, imports incorretos e leitura difícil do código.

## Decision

1. **Entidade de domínio:** `MonitoredApplication` em `HiveLogs.Domain.Applications`.
2. **Conceito público:** permanece **Application** em rotas HTTP (`/applications`), documentação de usuário e `docs/architecture.md`.
3. **DTOs e responses da API:** propriedades e rotas usam `application` / `applicationId` (sem expor o nome interno `MonitoredApplication`).
4. **Environment:** entidade `Environment` em `HiveLogs.Domain.Environments`; em arquivos que misturam ASP.NET e domínio, usar namespace qualificado quando necessário.
5. **EnvironmentKind:** classe estática com constantes `development`, `staging`, `production` — referência/documentação apenas; **não** enum fechado nem validação obrigatória de nome.

## Consequences

### Positivas

- Código de domínio e Infrastructure legível sem colisão com `HiveLogs.Application`.
- API e docs permanecem alinhados ao vocabulário do produto.
- Evolução futura (auth, keys por environment) apoia-se em modelo explícito.

### Negativas

- Mapeamento mental entre `MonitoredApplication` (código) e Application (produto/docs).
- Tabela SQL pode permanecer `applications` enquanto a classe C# é `MonitoredApplication`.

## Alternatives Considered

### Manter `Application` no Domain

**Rejeitado:** colisão frequente com a camada Application e com tipos do framework; exige aliases e disciplina extra em todo arquivo.

### Renomear para `HiveApplication`

**Rejeitado:** prefixo de produto no tipo de domínio sem ganho claro; `MonitoredApplication` descreve melhor o papel na observabilidade.

### Enum fechado para nomes de Environment

**Rejeitado:** projetos reais usam `qa`, `homolog`, `preview`, etc.; apenas constantes well-known como sugestão.
