using Medidas.Core;

namespace Medidas.Tests;

public class CatalogoDeUnidadesTests
{
    [Theory]
    [InlineData("C", Categoria.Temperatura)]
    [InlineData("°F", Categoria.Temperatura)]
    [InlineData("km", Categoria.Distancia)]
    [InlineData("lb", Categoria.Massa)]
    public void Obter_SimboloConhecido_RetornaUnidadeDaCategoriaCorreta(string simbolo, Categoria esperada)
    {
        var unidade = CatalogoDeUnidades.Obter(simbolo);

        Assert.Equal(esperada, unidade.Categoria);
    }

    [Fact]
    public void Obter_SimboloComEspacos_IgnoraEspacosNasBordas()
    {
        var unidade = CatalogoDeUnidades.Obter("  kg ");

        Assert.Equal("kg", unidade.Simbolo);
    }

    [Fact]
    public void Obter_SimboloDesconhecido_LancaUnidadeDesconhecidaException()
    {
        Action acao = () => CatalogoDeUnidades.Obter("légua");

        var excecao = Assert.Throws<UnidadeDesconhecidaException>(acao);
        Assert.Contains("légua", excecao.Message);
    }

    [Fact]
    public void Todas_Catalogo_PossuiSimbolosUnicos()
    {
        var simbolos = CatalogoDeUnidades.Todas.Select(u => u.Simbolo.ToUpperInvariant()).ToList();

        Assert.Equal(simbolos.Count, simbolos.Distinct().Count());
    }

    [Theory]
    [InlineData(Categoria.Temperatura, 3)]
    [InlineData(Categoria.Distancia, 8)]
    [InlineData(Categoria.Massa, 6)]
    public void DaCategoria_CategoriaInformada_RetornaApenasUnidadesDela(Categoria categoria, int quantidade)
    {
        var unidades = CatalogoDeUnidades.DaCategoria(categoria);

        Assert.Equal(quantidade, unidades.Count);
        Assert.All(unidades, u => Assert.Equal(categoria, u.Categoria));
    }
}
