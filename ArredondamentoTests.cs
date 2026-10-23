using Medidas.Core;

namespace Medidas.Tests;

public class ArredondamentoTests
{
    public static TheoryData<decimal, int, decimal> Casos => new()
    {
        { 2.345m, 2, 2.35m },
        { 2.344m, 2, 2.34m },
        { 2.5m, 0, 3m },
        { -2.5m, 0, -3m },
        { 1.23456789m, 4, 1.2346m },
        { 10m, 2, 10m },
    };

    [Theory]
    [MemberData(nameof(Casos))]
    public void Arredondar_ValorECasas_ArredondaPraLongeDoZeroNoMeio(decimal valor, int casas, decimal esperado)
    {
        var resultado = Arredondamento.Arredondar(valor, casas);

        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    public void Arredondar_CasasForaDoIntervalo_LancaArgumentOutOfRangeException(int casas)
    {
        Action acao = () => Arredondamento.Arredondar(1m, casas);

        Assert.Throws<ArgumentOutOfRangeException>(acao);
    }
}
