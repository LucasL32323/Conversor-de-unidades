# Conversor de Unidades

[![CI](https://github.com/LucasL32323/Conversor-de-unidades/actions/workflows/ci.yml/badge.svg)](https://github.com/LucasL32323/Conversor-de-unidades/actions/workflows/ci.yml)

Biblioteca e aplicação de linha de comando em C# (.NET 10) para converter **temperatura**, **distância** e **massa**. As conversões lineares usam uma tabela de fatores exatos com `decimal`, as temperaturas passam por Celsius como escala intermediária e todo resultado é arredondado de forma previsível.

## Como o TDD foi aplicado

O projeto foi construído no ciclo **Red → Green → Refactor**, sempre com o teste escrito antes do código de produção.

1. **Red** — o primeiro teste foi `Converter_Temperatura_RetornaValorConvertido` com o caso `0 °C → 32 °F`. Ele não compilava, pois `ConversorDeUnidades` ainda não existia.
2. **Green** — a implementação mínima aplicou a fórmula `C × 9/5 + 32`. Em seguida os casos `F → C`, `K → C` e `-40 °C = -40 °F` forçaram a generalização: converter a origem para Celsius e de Celsius para o destino (`EscalaDeTemperatura`).
3. **Refactor** — com os testes verdes, os símbolos espalhados em `switch` foram extraídos para `CatalogoDeUnidades`, e as distâncias/massas passaram a usar `TabelaDeFatores` (`valor × fatorOrigem ÷ fatorDestino`) em vez de uma fórmula por par de unidades.

Outros ciclos guiados por testes:

- `Converter_ValorFisicamenteImpossivel_LancaValorInvalidoException` (`-1 K`, `-273.16 °C`, `-5 m`) introduziu a validação de zero absoluto e de valores negativos.
- `Arredondar_ValorECasas_ArredondaPraLongeDoZeroNoMeio` (`2.345 → 2.35`, `-2.5 → -3`) fixou a regra `MidpointRounding.AwayFromZero` no lugar do arredondamento bancário padrão do .NET.
- `Converter_IdaEVolta_RetornaValorOriginal` garante que converter e desconverter devolve o valor original.

Padrões adotados nos testes: nomes `Metodo_Cenario_ResultadoEsperado`, blocos Arrange/Act/Assert separados por linha em branco e `[Theory]` com `TheoryData`/`MemberData` para cobrir vários casos com `decimal`.

## Arquitetura

```
ConversorDeUnidades/
├── src/
│   ├── Medidas.Core/          # Regras de negócio (sem dependências externas)
│   │   ├── Arredondamento.cs
│   │   ├── CatalogoDeUnidades.cs
│   │   ├── Categoria.cs
│   │   ├── ConversorDeUnidades.cs
│   │   ├── EscalaDeTemperatura.cs
│   │   ├── Excecoes.cs
│   │   ├── TabelaDeFatores.cs
│   │   └── Unidade.cs
│   └── Medidas.Cli/           # Interface de linha de comando
│       └── Program.cs
├── tests/
│   └── Medidas.Tests/         # Testes xUnit
├── .github/workflows/ci.yml
├── ConversorDeUnidades.slnx
├── Directory.Build.props
└── global.json
```

```
Medidas.Cli ──► Medidas.Core ◄── Medidas.Tests
```

## Regras de negócio

1. Categorias suportadas: temperatura (`C`, `F`, `K`), distância (`mm`, `cm`, `m`, `km`, `in`, `ft`, `yd`, `mi`) e massa (`mg`, `g`, `kg`, `t`, `oz`, `lb`).
2. Símbolos são aceitos sem diferenciar maiúsculas/minúsculas, com espaços nas bordas e com o prefixo `°` (`°F`).
3. Só é possível converter unidades da mesma categoria; caso contrário é lançada `ConversaoIncompativelException`.
4. Símbolo inexistente ou vazio lança `UnidadeDesconhecidaException`.
5. Distâncias e massas usam a tabela de fatores para a unidade base (metro e quilograma): `valor × fatorOrigem ÷ fatorDestino`.
6. Temperaturas são convertidas para Celsius e depois para a escala de destino.
7. Temperaturas abaixo do zero absoluto (−273,15 °C / 0 K / −459,67 °F) e distâncias ou massas negativas lançam `ValorInvalidoException`.
8. O resultado é arredondado para 2 casas decimais por padrão, configurável de 0 a 10, com meio arredondado para longe do zero.

## Decisões técnicas

- **`decimal` em todos os cálculos**: os fatores oficiais (`1 in = 0,0254 m`, `1 lb = 0,45359237 kg`) são representados sem erro binário, e `1 lb = 16 oz` resulta exato.
- **Tabela de fatores em vez de fórmulas por par**: adicionar uma unidade linear exige uma linha na tabela e uma no catálogo, sem mexer no conversor.
- **Celsius como pivô**: com 3 escalas bastam 2 funções por escala, em vez de 6 fórmulas.
- **Exceções de domínio específicas**: a CLI distingue erros de uso e devolve código de saída `1`; argumentos ausentes devolvem `2`.
- **`TreatWarningsAsErrors` e `Nullable`** habilitados para todos os projetos via `Directory.Build.props`.

## Como rodar

Pré-requisito: [.NET SDK 10](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/LucasL32323/Conversor-de-unidades.git
cd Conversor-de-unidades

dotnet test

dotnet run --project src/Medidas.Cli -- 100 C F        # 100 C = 212 F
dotnet run --project src/Medidas.Cli -- 1 mi km 4      # 1 mi = 1.6093 km
dotnet run --project src/Medidas.Cli -- 10 oz g        # 10 oz = 283.5 g
dotnet run --project src/Medidas.Cli -- --listar       # lista as unidades
```
