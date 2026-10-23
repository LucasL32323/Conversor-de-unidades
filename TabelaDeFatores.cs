namespace Medidas.Core;

/// <summary>
/// Fatores exatos para converter cada unidade linear na unidade base da sua categoria
/// (metro para distância, quilograma para massa).
/// </summary>
public static class TabelaDeFatores
{
    private static readonly Dictionary<string, decimal> Fatores = new()
    {
        ["mm"] = 0.001m,
        ["cm"] = 0.01m,
        ["m"] = 1m,
        ["km"] = 1000m,
        ["in"] = 0.0254m,
        ["ft"] = 0.3048m,
        ["yd"] = 0.9144m,
        ["mi"] = 1609.344m,

        ["mg"] = 0.000001m,
        ["g"] = 0.001m,
        ["kg"] = 1m,
        ["t"] = 1000m,
        ["oz"] = 0.028349523125m,
        ["lb"] = 0.45359237m,
    };

    public static decimal FatorParaBase(Unidade unidade) =>
        Fatores.TryGetValue(unidade.Simbolo, out var fator)
            ? fator
            : throw new ConversaoIncompativelException(
                $"A unidade '{unidade.Simbolo}' não é linear e não possui fator de conversão.");
}
