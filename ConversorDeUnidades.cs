namespace Medidas.Core;

public sealed class ConversorDeUnidades
{
    public decimal Converter(decimal valor, string de, string para, int casasDecimais = Arredondamento.CasasPadrao)
    {
        var origem = CatalogoDeUnidades.Obter(de);
        var destino = CatalogoDeUnidades.Obter(para);

        if (origem.Categoria != destino.Categoria)
        {
            throw new ConversaoIncompativelException(
                $"Não é possível converter {origem.Nome} ({origem.Categoria.Descricao()}) em {destino.Nome} ({destino.Categoria.Descricao()}).");
        }

        var convertido = origem.Categoria == Categoria.Temperatura
            ? ConverterTemperatura(valor, origem, destino)
            : ConverterLinear(valor, origem, destino);

        return Arredondamento.Arredondar(convertido, casasDecimais);
    }

    private static decimal ConverterTemperatura(decimal valor, Unidade origem, Unidade destino)
    {
        var celsius = EscalaDeTemperatura.ParaCelsius(valor, origem);

        if (celsius < EscalaDeTemperatura.ZeroAbsolutoEmCelsius)
        {
            throw new ValorInvalidoException(
                $"{valor} {origem.Simbolo} está abaixo do zero absoluto.");
        }

        return EscalaDeTemperatura.DeCelsius(celsius, destino);
    }

    private static decimal ConverterLinear(decimal valor, Unidade origem, Unidade destino)
    {
        if (valor < 0m)
        {
            throw new ValorInvalidoException(
                $"{origem.Categoria.Descricao()} não pode ser negativa: {valor} {origem.Simbolo}.");
        }

        return valor * TabelaDeFatores.FatorParaBase(origem) / TabelaDeFatores.FatorParaBase(destino);
    }
}
