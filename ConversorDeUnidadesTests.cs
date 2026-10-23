using Medidas.Core;

namespace Medidas.Tests;

public class ConversorDeUnidadesTests
{
    private readonly ConversorDeUnidades _conversor = new();

    public static TheoryData<decimal, string, string, decimal> ConversoesDeTemperatura => new()
    {
        { 0m, "C", "F", 32m },
        { 100m, "C", "F", 212m },
        { 100m, "C", "K", 373.15m },
        { 32m, "F", "C", 0m },
        { 98.6m, "F", "C", 37m },
        { -40m, "C", "F", -40m },
        { 0m, "K", "C", -273.15m },
        { 0m, "K", "F", -459.67m },
        { 300m, "K", "F", 80.33m },
        { 50m, "F", "K", 283.15m },
    };

    public static TheoryData<decimal, string, string, decimal> ConversoesDeDistancia => new()
    {
        { 1m, "km", "m", 1000m },
        { 1500m, "m", "km", 1.5m },
        { 1m, "m", "cm", 100m },
        { 25m, "mm", "cm", 2.5m },
        { 1m, "mi", "km", 1.61m },
        { 10m, "km", "mi", 6.21m },
        { 12m, "in", "cm", 30.48m },
        { 3m, "ft", "yd", 1m },
        { 1m, "yd", "in", 36m },
    };

    public static TheoryData<decimal, string, string, decimal> ConversoesDeMassa => new()
    {
        { 1m, "kg", "g", 1000m },
        { 2500m, "g", "kg", 2.5m },
        { 1m, "t", "kg", 1000m },
        { 500m, "mg", "g", 0.5m },
        { 1m, "kg", "lb", 2.2m },
        { 1m, "lb", "oz", 16m },
        { 10m, "oz", "g", 283.5m },
    };

    [Theory]
    [MemberData(nameof(ConversoesDeTemperatura))]
    public void Converter_Temperatura_RetornaValorConvertido(decimal valor, string de, string para, decimal esperado)
    {
        var resultado = _conversor.Converter(valor, de, para);

        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [MemberData(nameof(ConversoesDeDistancia))]
    public void Converter_Distancia_RetornaValorConvertido(decimal valor, string de, string para, decimal esperado)
    {
        var resultado = _conversor.Converter(valor, de, para);

        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [MemberData(nameof(ConversoesDeMassa))]
    public void Converter_Massa_RetornaValorConvertido(decimal valor, string de, string para, decimal esperado)
    {
        var resultado = _conversor.Converter(valor, de, para);

        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData("C")]
    [InlineData("km")]
    [InlineData("lb")]
    public void Converter_MesmaUnidade_RetornaValorArredondado(string unidade)
    {
        var resultado = _conversor.Converter(12.3456m, unidade, unidade);

        Assert.Equal(12.35m, resultado);
    }

    [Theory]
    [InlineData(4, 1.6093)]
    [InlineData(0, 2)]
    [InlineData(6, 1.609344)]
    public void Converter_CasasDecimaisInformadas_ArredondaNaPrecisaoPedida(int casas, double esperado)
    {
        var resultado = _conversor.Converter(1m, "mi", "km", casas);

        Assert.Equal((decimal)esperado, resultado);
    }

    [Fact]
    public void Converter_SimboloComCaixaDiferente_ConverteNormalmente()
    {
        var resultado = _conversor.Converter(1m, "KM", "M");

        Assert.Equal(1000m, resultado);
    }

    [Theory]
    [InlineData("C", "km")]
    [InlineData("kg", "m")]
    [InlineData("mi", "F")]
    public void Converter_CategoriasDiferentes_LancaConversaoIncompativelException(string de, string para)
    {
        Action acao = () => _conversor.Converter(1m, de, para);

        Assert.Throws<ConversaoIncompativelException>(acao);
    }

    [Theory]
    [InlineData("xyz", "m")]
    [InlineData("m", "parsec")]
    [InlineData("", "m")]
    [InlineData(" ", "kg")]
    public void Converter_UnidadeDesconhecida_LancaUnidadeDesconhecidaException(string de, string para)
    {
        Action acao = () => _conversor.Converter(1m, de, para);

        Assert.Throws<UnidadeDesconhecidaException>(acao);
    }

    [Theory]
    [InlineData(-1, "m", "km")]
    [InlineData(-0.01, "kg", "g")]
    [InlineData(-1, "K", "C")]
    [InlineData(-273.16, "C", "K")]
    [InlineData(-459.68, "F", "C")]
    public void Converter_ValorFisicamenteImpossivel_LancaValorInvalidoException(double valor, string de, string para)
    {
        Action acao = () => _conversor.Converter((decimal)valor, de, para);

        Assert.Throws<ValorInvalidoException>(acao);
    }

    [Theory]
    [InlineData("C", "F")]
    [InlineData("km", "mi")]
    [InlineData("kg", "oz")]
    public void Converter_IdaEVolta_RetornaValorOriginal(string de, string para)
    {
        var ida = _conversor.Converter(37.5m, de, para, 10);

        var volta = _conversor.Converter(ida, para, de);

        Assert.Equal(37.5m, volta);
    }
}
