# Guia — Handoff Pencil.dev → React

Aprendizados da **TS-005** (tela de setup inicial).

## Artefatos

| Arquivo | Uso |
|---------|-----|
| [docs/design.md](../design.md) | Design system (cores, tipografia, tom, dark-first) |
| [docs/ui/pencil.pen](../ui/pencil.pen) | Protótipo editável (Pencil MCP no Cursor) |
| [docs/Logo.svg](../Logo.svg) | Logo para `apps/web/src/assets/` |

Versionar `design.md` e `pencil.pen` na mesma feature que implementa a tela.

## Antes de codar

1. Ler `docs/design.md` (tokens e princípios).
2. Abrir `pencil.pen` via ferramentas Pencil (`get_editor_state`, `batch_get` no frame da tela).
3. Anotar no `shared-memory.md` da techspec:
   - frame id (ex. `kZpjm` — “Initial Setup Screen”)
   - textos, hierarquia de seções, CTA
   - lacunas vs requisitos de negócio

## Lacunas protótipo × produto

O protótipo pode **não** incluir tudo que o backend exige. Na TS-005:

| No protótipo | Obrigatório no produto |
|--------------|------------------------|
| Organization, admin, passwords | OK |
| Botão “Create instance” | Usar label do protótipo |
| — | Campo `setupPassword` (seção “Setup access”) |
| Link “Back to sign in” | Omitir durante `setupRequired` (evita loop) |

Documentar gaps em `shared-memory.md`; não assumir que o Pencil está completo.

## Implementação React

- Layout full-screen onboarding: fora do `AppLayout` com header genérico
- Cores do frame (ex. fundo `#020617`, card `#0F172A`, borda `#1E3A4A`) → mapear para Tailwind / tokens existentes
- Ícones: `lucide-react` alinhados ao protótipo
- Botão primário: gradiente cyan/teal/blue quando o protótipo indicar
- Caixa de segurança: copy do protótipo + “Passwords are never stored in the browser.”

## Ferramentas Pencil no agente

- `get_editor_state(include_schema: true)` na primeira interação com `.pen`
- `batch_get` com `nodeIds` ou patterns — evitar `readDepth` alto
- `get_screenshot` só para validação visual final (custo alto)
- **Não** usar `Read`/`Grep` em arquivos `.pen` (formato criptografado)

## Checklist de aceite visual

- [ ] Logo e wordmark no topo
- [ ] Título e subtítulo conforme protótipo
- [ ] Ordem e labels dos campos
- [ ] Estados de erro (bloco destructive, não só inline)
- [ ] Loading no submit
- [ ] Seções de negócio ausentes no protótipo adicionadas com helper text claro

## Referências

- Skill: `hivelogs-web-design-handoff`
- [web-dashboard-development.md](./web-dashboard-development.md)
