# Guia — API e banco em desenvolvimento local

Aprendizados de **TS-003**, **TS-005** e correções pós-implementação (CORS, migrations).

## Stack local típica

| Serviço | URL / porta | Config |
|---------|-------------|--------|
| API | `http://localhost:5054` | `launchSettings.json`, `appsettings.Development.json` |
| Web (Vite) | `http://localhost:5173` ou **5174** se 5173 ocupada | `apps/web/.env` |
| PostgreSQL / TimescaleDB | `localhost:6543` (ex. dev) | `infra/docker compose`, connection string `Default` |

O front deve usar a mesma porta da API em `VITE_API_BASE_URL`.

## CORS (obrigatório com dashboard)

O browser bloqueia chamadas de `localhost:5173`/`5174` → `localhost:5054` se a API não liberar a origem.

### Configuração

- Política: `WebDashboard` em `HiveLogs.Api/Infrastructure/Cors/CorsExtensions.cs`
- Registro: `AddHiveLogsCors` + `UseHiveLogsCors` em `Program.cs` (**antes** de `MapControllers`)
- Origens: `Cors:AllowedOrigins` em `appsettings.Development.json`

Incluir **ambas** as portas Vite comuns:

```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:5173",
    "http://127.0.0.1:5173",
    "http://localhost:5174",
    "http://127.0.0.1:5174"
  ]
}
```

**Sintoma:** rede ok no curl/Postman, falha no browser com erro CORS → reinicie a API após alterar origens.

## Setup self-hosted (teste manual)

| Config | Onde |
|--------|------|
| `Setup:Password` | `appsettings.Development.json` ou `HIVELOGS_SETUP_PASSWORD` |
| Senha no formulário web | Mesmo valor configurado no servidor |

Endpoints públicos: `GET /setup/status`, `POST /setup/initialize`. Após setup concluído, `setupRequired` passa a `false`.

## Migrations EF Core

### Pasta correta

Todas as migrations do `HiveLogsDbContext` ficam em:

```
apps/api/src/HiveLogs.Infrastructure/Migrations/
```

**Não** criar em `Persistence/Migrations/` — o EF pode gerar lá por engano se o diretório de trabalho ou tooling estiver inconsistente.

O `.csproj` da Infrastructure define:

```xml
<EfMigrationsDirectory>Migrations</EfMigrationsDirectory>
```

### Comandos

```bash
cd apps/api
dotnet ef database update \
  --project src/HiveLogs.Infrastructure \
  --startup-project src/HiveLogs.Api

dotnet ef migrations add NomeDescritivo \
  --project src/HiveLogs.Infrastructure \
  --startup-project src/HiveLogs.Api
```

### Histórico e renomeação

- O ID da migration (`20260604220802_Nome`) fica em `__EFMigrationsHistory` — **não renomear** após aplicar em ambiente compartilhado.
- Se o índice/tabela já existir manualmente, `database update` pode falhar com “already exists”; alinhar histórico ou ajustar migration antes de merge.

### Migrations atuais (referência)

| Migration | Conteúdo |
|-----------|----------|
| `20260602214111_InitialCoreDomain` | organizations, applications, environments |
| `20260604120000_AddSelfHostedAccessModel` | users, organization_members, setup_state |
| `20260604220802_SetupAndWebLatestChanges` | índice `organization_members.user_id` |

## Verificação rápida

```bash
cd apps/api
dotnet build
dotnet test
curl -s http://localhost:5054/health
curl -s http://localhost:5054/setup/status
```

## Referências

- [apps/api/README.md](../../apps/api/README.md)
- Skill: `hivelogs-ef-migrations`
