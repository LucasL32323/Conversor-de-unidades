using System.Globalization;
using Medidas.Core;

var cultura = CultureInfo.InvariantCulture;

if (args.Length == 1 && args[0] is "--listar" or "-l")
{
    foreach (var grupo in CatalogoDeUnidades.Todas.GroupBy(u => u.Categoria))
    {
        Console.WriteLine($"{grupo.Key.Descricao()}:");
        foreach (var unidade in grupo)
        {
            Console.WriteLine($"  {unidade.Simbolo,-3} {unidade.Nome}");
        }
    }

    return 0;
}

if (args.Length is not (3 or 4))
{
    Console.Error.WriteLine("Uso: Medidas.Cli <valor> <de> <para> [casas-decimais]");
    Console.Error.WriteLine("     Medidas.Cli --listar");
    Console.Error.WriteLine("Ex.: Medidas.Cli 100 C F");
    return 2;
}

if (!decimal.TryParse(args[0], NumberStyles.Number, cultura, out var valor))
{
    Console.Error.WriteLine($"Valor inválido: '{args[0]}'. Use ponto como separador decimal.");
    return 1;
}

var casas = Arredondamento.CasasPadrao;
if (args.Length == 4 && !int.TryParse(args[3], NumberStyles.Integer, cultura, out casas))
{
    Console.Error.WriteLine($"Casas decimais inválidas: '{args[3]}'.");
    return 1;
}

try
{
    var resultado = new ConversorDeUnidades().Converter(valor, args[1], args[2], casas);
    var origem = CatalogoDeUnidades.Obter(args[1]);
    var destino = CatalogoDeUnidades.Obter(args[2]);
    Console.WriteLine($"{valor.ToString(cultura)} {origem.Simbolo} = {resultado.ToString("0.##########", cultura)} {destino.Simbolo}");
    return 0;
}
catch (Exception ex) when (ex is UnidadeDesconhecidaException
                               or ConversaoIncompativelException
                               or ValorInvalidoException
                               or ArgumentOutOfRangeException)
{
    var mensagem = ex is ArgumentOutOfRangeException fora ? fora.Message.Split(" (Parameter")[0] : ex.Message;
    Console.Error.WriteLine($"Erro: {mensagem}");
    return 1;
}
