using Medidas.Core;

namespace Medidas.Tests;

public class TabelaDeFatoresTests
{
    public static TheoryData<string, decimal> Fatores => new()
    {
        { "m", 1m },
        { "km", 1000m },
        { "in", 0.0254m },
        { "mi", 1609.344m },
        { "kg", 1m },
        { "g", 0.001m },
        { "lb", 0.45359237m },
    };

    [Theory]
    [MemberData(nameof(Fatores))]
    public void FatorParaBase_UnidadeLinear_RetornaFatorExato(string simbolo, decimal esperado)
    {
        var unidade = CatalogoDeUnidades.Obter(simbolo);

        var fator = TabelaDeFatores.FatorParaBase(unidade);

        Assert.Equal(esperado, fator);
    }

    [Fact]
    public void FatorParaBase_Temperatura_LancaConversaoIncompativelException()
    {
        var celsius = CatalogoDeUnidades.Obter("C");

        Action acao = () => TabelaDeFatores.FatorParaBase(celsius);

        Assert.Throws<ConversaoIncompativelException>(acao);
    }

    [Fact]
    public void Tabela_TodasAsUnidadesLineares_PossuemFatorPositivo()
    {
        var lineares = CatalogoDeUnidades.Todas.Where(u => u.Categoria != Categoria.Temperatura);

        Assert.All(lineares, u => Assert.True(TabelaDeFatores.FatorParaBase(u) > 0m));
    }
}
