namespace Medidas.Core;

public static class CatalogoDeUnidades
{
    public static IReadOnlyList<Unidade> Todas { get; } =
    [
        new("C", "Celsius", Categoria.Temperatura),
        new("F", "Fahrenheit", Categoria.Temperatura),
        new("K", "Kelvin", Categoria.Temperatura),

        new("mm", "Milímetro", Categoria.Distancia),
        new("cm", "Centímetro", Categoria.Distancia),
        new("m", "Metro", Categoria.Distancia),
        new("km", "Quilômetro", Categoria.Distancia),
        new("in", "Polegada", Categoria.Distancia),
        new("ft", "Pé", Categoria.Distancia),
        new("yd", "Jarda", Categoria.Distancia),
        new("mi", "Milha", Categoria.Distancia),

        new("mg", "Miligrama", Categoria.Massa),
        new("g", "Grama", Categoria.Massa),
        new("kg", "Quilograma", Categoria.Massa),
        new("t", "Tonelada", Categoria.Massa),
        new("oz", "Onça", Categoria.Massa),
        new("lb", "Libra", Categoria.Massa),
    ];

    private static readonly Dictionary<string, Unidade> PorSimbolo =
        Todas.ToDictionary(u => u.Simbolo, StringComparer.OrdinalIgnoreCase);

    public static Unidade Obter(string simbolo)
    {
        var normalizado = (simbolo ?? string.Empty).Trim().TrimStart('°');

        return PorSimbolo.TryGetValue(normalizado, out var unidade)
            ? unidade
            : throw new UnidadeDesconhecidaException(simbolo ?? string.Empty);
    }

    public static IReadOnlyList<Unidade> DaCategoria(Categoria categoria) =>
        Todas.Where(u => u.Categoria == categoria).ToList();
}
