namespace Medidas.Core;

internal static class EscalaDeTemperatura
{
    public const decimal ZeroAbsolutoEmCelsius = -273.15m;

    public static decimal ParaCelsius(decimal valor, Unidade origem) => origem.Simbolo switch
    {
        "C" => valor,
        "F" => (valor - 32m) * 5m / 9m,
        "K" => valor + ZeroAbsolutoEmCelsius,
        _ => throw new UnidadeDesconhecidaException(origem.Simbolo),
    };

    public static decimal DeCelsius(decimal celsius, Unidade destino) => destino.Simbolo switch
    {
        "C" => celsius,
        "F" => celsius * 9m / 5m + 32m,
        "K" => celsius - ZeroAbsolutoEmCelsius,
        _ => throw new UnidadeDesconhecidaException(destino.Simbolo),
    };
}
