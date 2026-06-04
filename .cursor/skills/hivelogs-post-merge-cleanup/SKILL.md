---
name: hivelogs-post-merge-cleanup
description: >-
  Limpa o repositório após merge de PR de techspec: volta para main, puxa
  atualização, apaga branch local e remota da feature. Use quando o usuário
  disser que o PR foi mergeado, aprovado, ou pedir "pós-merge", "de sempre",
  "voltar pra main" ou limpar branch da feature.
disable-model-invocation: true
---

# HiveLogs — Pós-merge (limpeza de branch)

Rotina após **merge do PR único** da techspec em `main`. Não substitui o fluxo de feature (`hivelogs-feature-workflow`); executa **depois** do merge.

## Pré-requisitos

- PR da techspec já mergeado em `main` (usuário confirmou ou GitHub indica merged).
- Saber o nome da branch da feature (ex. `feature/TS-005-initial-setup-screen`).

## Passos (ordem fixa)

```bash
cd <repo-root>

# 1. Voltar para main
git checkout main

# 2. Atualizar main local
git pull origin main

# 3. Apagar branch local da feature
git branch -d feature/TS-NNN-slug

# 4. Apagar branch remota no GitHub
git push origin --delete feature/TS-NNN-slug
```

Se `git branch -d` falhar por commits não mergeados, **não** usar `-D` sem confirmar com o usuário — pode indicar merge incompleto ou branch errada.

## Verificação final

```bash
git status
git branch -a
```

Esperado:

- Branch atual: `main`
- `Your branch is up to date with 'origin/main'`
- Branch `feature/TS-NNN-slug` ausente em local e em `remotes/origin/`

## Opcional (só se ainda não feito no merge)

| Item | Onde |
|------|------|
| Techspec `status: Implemented` | `docs/techspecs/TS-NNN-*/techspec.md` |
| Índice techspecs | `docs/techspecs/README.md` |
| Roadmap checkbox | `docs/roadmap.md` |

Isso costuma estar no PR; não reabrir commits na `main` só para docs se já mergeado.

## Regras

- **Nunca** `git push --force` em `main` / `master`
- **Nunca** apagar branch sem o usuário ter confirmado merge (ou evidência via `gh pr view`)
- **Não** fazer commit nesta rotina — só git checkout/pull/branch delete
- Push em `main` direto continua proibido pelo fluxo do projeto

## Diagnóstico rápido

| Situação | Ação |
|----------|------|
| `pull` com conflitos | Parar; resolver em branch separada, não na feature mergeada |
| `branch -d` recusada | Verificar se PR realmente entrou em `main` (`git log main --oneline -5`) |
| Remote delete 404 | Branch remota já apagada (GitHub "Delete branch" no merge) — OK |
| Ainda em feature branch | `checkout main` antes do pull |

## Prompt sugerido

```
PR #N da TS-NNN foi mergeado. Execute hivelogs-post-merge-cleanup
para a branch feature/TS-NNN-slug.
```

## Referências

- [docs/techspecs/README.md](../../../docs/techspecs/README.md) — Git workflow
- `hivelogs-feature-workflow` — fluxo antes do merge
- `hivelogs-commit-workflow` — fluxo por task (antes do PR)
