# Bridge Build

Demo de um jogo de construção de pontes feito em Unity para o Projeto Introdução ao Unity (Insper, Jogos Digitais 2026).

**Autor:** [SEU NOME COMPLETO]
**Jogar no navegador:** https://matheusluciano99.itch.io/bridge-build

## Como jogar

Construa uma ponte entre os penhascos. Ao testar, o carro anda sozinho até a bandeira; ele precisa chegar antes que o tempo acabe.

| Ação | Teclado e mouse |
|---|---|
| Criar viga | Clicar perto de um ponto e arrastar |
| Apagar viga | Botão direito sobre a viga |
| Testar / voltar a construir | **Espaço** ou botão "Testar" |

- Cada viga custa conforme o comprimento e o orçamento da fase é limitado.
- Durante o teste as vigas ficam vermelhas quando estão perto de quebrar.
- Se o carro cair, você perde um carro e volta a construir com a ponte intacta. Sem carros ou sem tempo, a fase é perdida.
- Ao vencer, a tela mostra o tempo final e a pontuação (orçamento que sobrou + tempo restante × 10).

## Fases

1. **O Vão** — vão de 8 unidades, orçamento de $3100, 150 s.
2. **O Pilar** — vão de 10 unidades com um pilar no meio, orçamento de $2300, 150 s.

## Estrutura do código

| Pasta | Scripts | Responsabilidade |
|---|---|---|
| `Assets/Scripts/Ponte` | `BridgeBuilder`, `Beam`, `Node` | Construção da ponte com o mouse, vigas com `HingeJoint2D` e pontos de ligação |
| `Assets/Scripts/Veiculo` | `Carro` | Carro com `WheelJoint2D` que acelera sozinho no teste |
| `Assets/Scripts/Jogo` | `GameManager`, `Bandeira` | Tempo, orçamento, carros, vitória e derrota |
| `Assets/Scripts/UI` | `MenuController` | Menu inicial |

## Créditos

- Código, cenas e fases: feitos pelo autor, com assistência de IA (Claude, da Anthropic) na escrita do código e no ajuste da física.
- Sprites: formas básicas (quadrado, círculo e triângulo) do pacote 2D do Unity, recoloridas.
- Sons e música: gerados por código com o script `Tools/gerar_sons.py` (síntese em Python, sem amostras externas), escrito com assistência de IA (Claude, da Anthropic).
- Fonte: LiberationSans, incluída no TextMesh Pro do Unity.
- Base do menu e da tela de fim: tutorial "Introdução Rápida" da disciplina.
