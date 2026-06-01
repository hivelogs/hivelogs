# Templates do HiveLogs

Templates para artefatos recorrentes de desenvolvimento. Copie o arquivo adequado, preencha os campos e salve no local indicado.

## Quando usar cada template

| Template | Destino | Skill relacionada |
|----------|---------|-------------------|
| [adr-template.md](./adr-template.md) | `docs/adr/NNN-titulo.md` | `hivelogs-adr` |
| [issue-template.md](./issue-template.md) | Issue tracker (copiar corpo) | `hivelogs-mvp-scope` |
| [pr-description-template.md](./pr-description-template.md) | Descrição do PR | `hivelogs-security-review` |
| [module-readme-template.md](./module-readme-template.md) | `apps/*/README.md` ou `packages/*/README.md` | `hivelogs-module-scaffold` |

## Numeração de ADRs

1. Liste ADRs existentes em `docs/adr/`.
2. Use o próximo número sequencial com três dígitos: `002-`, `003-`, etc.
3. Título em kebab-case no nome do arquivo.

## Idioma

- ADRs e issues internas: **PT-BR**
- README raiz e documentação de usuário final: **inglês** (ver [README.md](../../README.md))
