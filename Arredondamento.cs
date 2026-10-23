namespace Medidas.Core;

public static class Arredondamento
{
    public const int CasasPadrao = 2;
    public const int CasasMaximas = 10;

    public static decimal Arredondar(decimal valor, int casasDecimais)
    {
        if (casasDecimais is < 0 or > CasasMaximas)
        {
            throw new ArgumentOutOfRangeException(
                nameof(casasDecimais), casasDecimais, $"As casas decimais devem estar entre 0 e {CasasMaximas}.");
        }

        return Math.Round(valor, casasDecimais, MidpointRounding.AwayFromZero);
    }
}
