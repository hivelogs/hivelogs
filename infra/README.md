# infra

## Propósito

Infraestrutura de desenvolvimento local e base para deploy. Centraliza Docker Compose e variáveis de ambiente por serviço.

## Responsabilidades

- `docker-compose.yml` com TimescaleDB e placeholders para apps.
- `.env.example` com variáveis separadas por serviço.
- Documentação de como subir o ambiente local.

## Serviços no Docker Compose

| Serviço | Status atual |
|---------|--------------|
| `timescaledb` | Ativo — imagem `timescale/timescaledb:latest-pg16` |
| `api` | Placeholder (comentado) |
| `web` | Placeholder (comentado) |
| `worker` | Placeholder (comentado) |
| `mcp` | Placeholder (comentado) |

Os serviços de aplicação serão habilitados quando os projetos em `apps/` forem inicializados e os Dockerfiles forem adicionados.

## Como usar (apenas banco)

```bash
cd infra
cp .env.example .env
# Edite .env com senhas seguras
docker compose up timescaledb -d
```

Verificar saúde:

```bash
docker compose ps
docker compose logs timescaledb
```

## Variáveis de ambiente

Cada serviço possui seção própria em `.env.example`:

- `TIMESCALEDB_*` — banco de dados
- `API_*` — apps/api
- `WEB_*` — apps/web
- `WORKER_*` — apps/worker
- `MCP_*` — apps/mcp

**Nunca** commite o arquivo `.env` com valores reais. Ele está no `.gitignore`.

## O que NÃO deve conter

- Código-fonte de aplicações.
- Migrations de banco (ficam em `apps/api` quando implementadas).
- Secrets ou senhas reais commitadas.
- Configuração de produção/cloud (CI/CD, Terraform — fases futuras).
