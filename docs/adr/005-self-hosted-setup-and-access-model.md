# ADR 005 - Modelo Self-Hosted de Setup e Acesso

## Status

Accepted

## Context

O HiveLogs é uma plataforma corporativa/self-hosted. Não há cadastro público de usuários como em SaaS multi-tenant aberto. Cada instalação precisa de um fluxo controlado de primeira configuração que crie organização e administrador inicial, sem expor registro aberto na API.

Decisões de produto já definidas no planning da TS-003: sem `POST /auth/register`, setup protegido por senha de ambiente, admin define a própria senha no setup, e o backend nunca gera nem retorna senhas em texto.

## Decision

### Sem registro público

Não existirá endpoint público de auto-cadastro (`POST /auth/register`). Usuários adicionais serão criados por administradores em feature futura (TS-005 prevista).

### Setup inicial obrigatório

Estados de instalação:

- `SetupRequired` — setup ainda não concluído; apenas endpoints de setup/status permitidos (nesta feature, sem middleware global de bloqueio).
- `Configured` — setup concluído; `POST /setup/initialize` retorna `409 setup.already_completed`.

O status é persistido explicitamente em `setup_state` (não inferido apenas por existência de registros). A tabela é **singleton**: uma única linha com ID fixo (`SetupState.SingletonId`). `GET /setup/status` não grava no banco; ausência de linha implica `SetupRequired`.

### Proteção do setup por variável de ambiente

Senha/token de setup via configuração:

- Variável de ambiente: `HIVELOGS_SETUP_PASSWORD`
- Configuração equivalente: `Setup:Password`

Usada **somente** para autorizar `POST /setup/initialize`. Não é a senha do admin. Alterar a variável após setup concluído **não** altera dados no banco.

### Admin inicial no setup

O primeiro setup cria em transação única:

1. Organização inicial
2. Usuário admin (`UserRole.Admin`, `MustChangePassword = false`)
3. Vínculo `OrganizationMember` com papel `Owner`
4. `setup_state` marcado como concluído

### Backend não gera nem retorna senha

- O backend **nunca** gera senha temporária.
- O backend **nunca** retorna senha, hash ou `setupPassword` em responses.
- Em fluxos futuros de criação/reset por admin: o frontend pode gerar senha localmente; o backend recebe, valida força mínima, persiste apenas hash e define `MustChangePassword = true`.

### Hash de senha

Usar `PasswordHasher<T>` do ASP.NET Core Identity como fundação inicial, **sem** adotar ASP.NET Identity completo nesta feature. Interface `IPasswordHasher` na Application; implementação na Infrastructure.

### MustChangePassword

Campo em `User`:

- Setup inicial: `false` (admin acabou de definir senha final).
- Criação/reset futuro por admin: `true` (feature 004/005).

### Membership organização–usuário

Entidade `OrganizationMember` desde o início (evita `PrimaryOrganizationId` que dificulta multi-org). Papéis de membership: `Owner`, `Admin`, `Member`. Admin inicial recebe `Owner` na organização criada.

### Reset do único admin

**Fora de escopo** desta ADR/feature. Documentado como decisão futura (recovery token, CLI no container, SQL manual, recovery code no setup). Requer ADR/techspec dedicada por impacto de segurança.

## Consequences

### Positivas

- Modelo alinhado a deploy self-hosted corporativo.
- Fundação clara para JWT/login (feature 004) e gestão de usuários (feature 005).
- Senhas nunca trafegam de volta ao cliente após persistência.
- Estado de setup explícito e idempotente no sentido de “não repetir setup”.

### Negativas

- Operador deve configurar `HIVELOGS_SETUP_PASSWORD` antes do primeiro setup.
- Perda de acesso do único admin exige procedimento manual futuro (ainda não implementado).

## Alternatives Considered

### Inferir setup por existência de admin/organização

**Rejeitada:** frágil se dados parciais ou migrações; tabela `setup_state` deixa o contrato explícito.

### Registro público com convite por email

**Rejeitada:** incompatível com modelo self-hosted corporativo do produto.

### ASP.NET Identity completo

**Rejeitada no MVP:** complexidade prematura; hash isolado via `PasswordHasher<T>` é suficiente para TS-003.

### Argon2id customizado

**Adiada:** `PasswordHasher<T>` do ASP.NET Core atende fundação; troca de algoritmo pode ser ADR futura se exigido.
